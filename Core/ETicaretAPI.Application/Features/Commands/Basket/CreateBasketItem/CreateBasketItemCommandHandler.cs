using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Basket.CreateBasketItem
{
    public class CreateBasketItemCommandHandler : IRequestHandler<CreateBasketItemCommandRequest, CreateBasketItemCommandResponse>
    {
        readonly IBasketService _basketService;

        public CreateBasketItemCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<CreateBasketItemCommandResponse> Handle(CreateBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.CreateBasketItemAsync(new()
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });

            return new();
        }
    }
}
