using ETicaretAPI.Application.DTOs.Baskets;
using ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task CreateBasketItemAsync(CreateBasketItem basketItem);
        public Task UpdateBasketItemAsync(UpdateBasketItem basketItem);
        public Task RemoveBasketItemAsync(string basketItemId);
    }
}
