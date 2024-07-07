using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    internal class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IConfiguration _configuration;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            var products = product?.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
            {
                Id = p.Id,
                FileName = p.Name,
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}"
            }).ToList();

            return products;
        }
    }
}
