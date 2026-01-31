using GS1Serialization.Domain.Entities;
using GS1Serialization.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace GS1Serialization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-test-data")]
        public async Task<IActionResult> CreateTestData()
        {
            var customer = new Customer
            {
                CompanyName = "Acme İlaç A.Ş.",
                GLN = "8680000000001",
                Description = "Ana Tedarikçi"
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                CustomerId = customer.Id,
                Name = "Aspirin 500mg",
                GTIN = "08680000000000" 
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Test verisi eklendi!", ProductId = product.Id });
        }

    }
}
