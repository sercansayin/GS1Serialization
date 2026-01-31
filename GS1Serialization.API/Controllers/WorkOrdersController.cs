using GS1Serialization.Application.DTOs;
using GS1Serialization.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GS1Serialization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        /// <summary>
        /// Yeni bir üretim iş emri oluşturur ve seri numaralarını basar.
        /// </summary>
        /// <param name="request">İş emri detayları</param>
        /// <returns>Oluşturulan iş emri özeti</returns>
        [HttpPost]
        [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWorkOrder([FromBody] CreateWorkOrderRequest request)
        {
                var response = await _workOrderService.CreateWorkOrderAsync(request);
                return CreatedAtAction(nameof(GetWorkOrder), new { id = response.WorkOrderId }, response);
        }

        /// <summary>
        /// ID'si verilen iş emrinin detaylarını ve üretilen paketleri getirir.
        /// </summary>
        /// <param name="id">İş Emri ID</param>
        /// <returns>Detaylı rapor</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkOrder(int id)
        {
            var response = await _workOrderService.GetWorkOrderByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Mevcut ürünleri veya kolileri bir üst pakette (Koli/Palet) birleştirir (Agregasyon).
        /// </summary>
        [HttpPost("aggregate")]
        [ProducesResponseType(typeof(AggregationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AggregatePackages([FromBody] CreateAggregationRequest request)
        {
            var response = await _workOrderService.AggregatePackagesAsync(request);
            return Ok(response);
        }
    }
}
