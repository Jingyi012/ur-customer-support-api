using Application.DTOs.Product;
using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<PagedResponse<List<ProductResponseDto>>>
    {
        public int PageNumber { get; set; } = 1; // page number
        public int PageSize { get; set; } = 20;
        public string? Name { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public bool? IsActive { get; set; }
        public SortByOption? SortBy { get; set; }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<List<ProductResponseDto>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<ProductResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllProductsParameter>(request);
            var products = await _productRepository.GetAllProducts(validFilter);
            var mappedProducts = _mapper.Map<List<ProductResponseDto>>(products.Data);

            var productsResponse = new PagedResponse<List<ProductResponseDto>>(
                mappedProducts,
                products.PageNumber,
                products.PageSize,
                products.TotalCount
            );

            return productsResponse;
        }
    }
}
