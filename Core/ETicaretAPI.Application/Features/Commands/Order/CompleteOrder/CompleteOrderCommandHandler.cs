using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IMailService _mailService;

        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool Successed, CompletedOrderDTO Dto) result = await _orderService.CompleteOrderAsync(request.Id);
            if(result.Successed)
                await _mailService.SendCompletedOrderMailAsync(result.Dto.Email, result.Dto.OrderCode, result.Dto.OrderDate, result.Dto.UserName);

            return new();
        }
    }
}
