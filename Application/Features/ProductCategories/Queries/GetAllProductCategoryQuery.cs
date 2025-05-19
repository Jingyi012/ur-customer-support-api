using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Queries
{
    public class GetAllProductCategoryQuery : IRequest<Response<List<ProductCategoryResponseDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
    public class GetAllProductCategoryQueryHandler : IRequestHandler<GetAllProductCategoryQuery, Response<List<ProductCategoryResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IProductCategoryRepositoryAsync _productCategoryRepository;

        public GetAllProductCategoryQueryHandler(IMapper mapper, IProductCategoryRepositoryAsync productCategoryRepository)
        {
            _mapper = mapper;
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<Response<List<ProductCategoryResponseDto>>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
        {
            if(request.PageNumber == null || request.PageSize == null)
            {
                var productCategories = await _productCategoryRepository.GetAllAsync();
                var mappedResponse = _mapper.Map<List<ProductCategoryResponseDto>>(productCategories);
                return new Response<List<ProductCategoryResponseDto>>(mappedResponse);
            }
            else
            {
                var productCategories = await _productCategoryRepository.GetAllProductCategory(request.PageNumber ?? 1, request.PageSize ?? 20);
                var mappedResponse = _mapper.Map<List<ProductCategoryResponseDto>>(productCategories.Data);
                return new PagedResponse<List<ProductCategoryResponseDto>>(mappedResponse, productCategories.PageNumber, productCategories.PageSize, productCategories.TotalCount);
            }
            
        }
    }
}
