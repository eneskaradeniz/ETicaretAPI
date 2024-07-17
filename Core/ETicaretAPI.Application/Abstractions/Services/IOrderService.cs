using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.RequestParameters;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<ListOrder> GetAllOrdersAsync(Pagination pagination);
        Task<SingleOrder> GetOrderById(string id);
        Task CreateOrderAsync(CreateOrder createOrder);
        Task CompleteOrderAsync(string id);
    }
}
