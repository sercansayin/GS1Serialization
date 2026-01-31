using GS1Serialization.Application.DTOs;

namespace GS1Serialization.Application.Interfaces
{
    public interface IWorkOrderService
    {
        Task<WorkOrderResponse> CreateWorkOrderAsync(CreateWorkOrderRequest request);
        Task<WorkOrderResponse> GetWorkOrderByIdAsync(int id);
        Task<AggregationResponse> AggregatePackagesAsync(CreateAggregationRequest request);
    }
}
