using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetOrderById(request.Id);
            return new()
            {
                Id = data.Id,
                Description = data.Description,
                Address = data.Address,
                OrderCode = data.OrderCode,
                CreatedDate = data.CreatedDate,
                BasketItems = data.BasketItems,
                Completed = data.Completed,
            };
        }
    }
}
