using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.RequestParameters;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<ListOrder> GetAllOrdersAsync(Pagination pagination);
        Task CreateOrderAsync(CreateOrder createOrder);
    }
}
