using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.RequestParameters;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<ListOrder> GetAllOrdersAsync(int page, int size);
        Task<SingleOrder> GetOrderById(string id);
        Task CreateOrderAsync(CreateOrder createOrder);
        Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
    }
}
