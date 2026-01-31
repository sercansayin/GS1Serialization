using GS1Serialization.Application.Interfaces;
using GS1Serialization.Application.DTOs;
using GS1Serialization.Infrastructure.Persistence;
using GS1Serialization.Domain.Entities;
using GS1Serialization.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using GS1Serialization.Domain.Exceptions;

namespace GS1Serialization.Infrastructure.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly AppDbContext _context;
        private readonly IGS1GeneratorService _gs1Generator;

        public WorkOrderService(AppDbContext context, IGS1GeneratorService gs1Generator)
        {
            _context = context;
            _gs1Generator = gs1Generator;
        }

        public async Task<WorkOrderResponse> CreateWorkOrderAsync(CreateWorkOrderRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                throw new NotFoundException(nameof(Product), request.ProductId);

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var workOrder = new WorkOrder
                {
                    ProductId = request.ProductId,
                    WorkOrderCode = $"WO-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                    TargetQuantity = request.Quantity,
                    LotNumber = request.LotNumber,
                    ExpireDate = request.ExpireDate,
                    Status = WorkOrderStatus.Created,
                    CreatedDate = DateTime.Now
                };

                _context.WorkOrders.Add(workOrder);
                await _context.SaveChangesAsync(); // ID oluşması için 

                var packages = new List<Package>();
                var currentSerial = request.SerialNumberStart;

                for (var i = 0; i < request.Quantity; i++)
                {
                    var serialStr = currentSerial.ToString();

                    var package = new Package
                    {
                        WorkOrderId = workOrder.Id,
                        SerialNumber = serialStr,
                        Level = PackageLevel.Item, 
                        CreatedDate = DateTime.Now
                    };

                    packages.Add(package);
                    currentSerial++;
                }

                await _context.Packages.AddRangeAsync(packages);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new WorkOrderResponse
                {
                    WorkOrderId = workOrder.Id,
                    WorkOrderCode = workOrder.WorkOrderCode,
                    ProducedQuantity = packages.Count,
                    Status = workOrder.Status.ToString()
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task<WorkOrderResponse> GetWorkOrderByIdAsync(int id)
        {
            var wo = await _context.WorkOrders
                .Include(x => x.Product)
                .ThenInclude(p => p!.Customer) 
                .Include(x => x.Packages)
                .AsNoTracking() 
                .FirstOrDefaultAsync(x => x.Id == id);

            if (wo == null) return null!;

            var productDto = new ProductDto
            {
                Name = wo.Product!.Name,
                GTIN = wo.Product.GTIN,
                Description = $"{wo.Product.Customer?.CompanyName} - {wo.Product.Name}"
            };
            var allPackages = wo.Packages.Select(p => new PackageDto
            {
                SerialNumber = p.SerialNumber,
                SSCC = p.SSCC,
                Level = p.Level.ToString(),
                FullGS1String = p.Level == Domain.Enum.PackageLevel.Item
                    ? _gs1Generator.GenerateGS1String(wo.Product.GTIN, wo.LotNumber, wo.ExpireDate, p.SerialNumber)
                    : p.SSCC, 
            }).ToList();

            return new WorkOrderResponse
            {
                WorkOrderId = wo.Id,
                WorkOrderCode = wo.WorkOrderCode,
                ProducedQuantity = wo.Packages.Count,
                Status = wo.Status.ToString(),
                Product = productDto,
                Packages = allPackages
            };
        }

        public async Task<AggregationResponse> AggregatePackagesAsync(CreateAggregationRequest request)
        {
            // 1. Transaction Başlat (Veri bütünlüğü için şart)
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. child paketleri veritabanından çek 
                var childPackages = await _context.Packages
                    .Where(p => request.ChildSerialNumbers.Contains(p.SerialNumber) && p.WorkOrderId == request.WorkOrderId)
                    .ToListAsync();

                // 3. Validasyonlar
                if (childPackages.Count != request.ChildSerialNumbers.Count)
                {
                    // Veritabanında bulunamayan seri numaraları var!
                    var missing = request.ChildSerialNumbers.Except(childPackages.Select(p => p.SerialNumber));
                    throw new BusinessException($"Şu seri numaraları bulunamadı veya iş emrine ait değil: {string.Join(", ", missing)}");
                }

                if (childPackages.Any(p => p.ParentPackageId != null))
                {
                    throw new BusinessException("Seçilen ürünlerden bazıları zaten paketlenmiş! Önce paket bozulmalı.");
                }

                // 4. Yeni Ebeveyn (Parent) Paketi Oluştur
                // SSCC ve Yeni Seri Numarası üretimi
                var parentSerial = DateTime.Now.Ticks.ToString()[10..]; // Basit unique üretim
                var sscc = $"00{parentSerial.PadLeft(16, '0')}";

                var parentPackage = new Package
                {
                    WorkOrderId = request.WorkOrderId,
                    SerialNumber = parentSerial,
                    SSCC = sscc, // Sadece Koli ve Palet için SSCC olur
                    Level = request.TargetLevel, // Box veya Pallet
                    CreatedDate = DateTime.Now
                };

                _context.Packages.Add(parentPackage);
                await _context.SaveChangesAsync(); // ID oluşsun diye save ediyoruz

                // 5. İlişkiyi Kur (Çocukları bu paketin içine at)
                foreach (var child in childPackages)
                    child.ParentPackageId = parentPackage.Id;

                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new AggregationResponse
                {
                    ParentSerialNumber = parentSerial,
                    SSCC = sscc,
                    ChildCount = childPackages.Count
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
