using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
    {
        public Pagination Pagination { get; set; }
    }
}
