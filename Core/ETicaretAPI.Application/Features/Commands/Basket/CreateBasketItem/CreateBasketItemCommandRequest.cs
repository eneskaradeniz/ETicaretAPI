using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Basket.CreateBasketItem
{
    public class CreateBasketItemCommandRequest : IRequest<CreateBasketItemCommandResponse>
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
