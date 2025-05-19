using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Commands
{
    public class DeleteProductCategoryByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCategoryByIdCommandHandler : IRequestHandler<DeleteProductCategoryByIdCommand, Response<int>>
    {
        private readonly IProductCategoryRepositoryAsync _productCategoryRepository;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IFileService _fileService; 
        private readonly string _baseUrl;
        public DeleteProductCategoryByIdCommandHandler(IProductCategoryRepositoryAsync productCategoryRepository, IProductRepositoryAsync productRepository, IFileService fileService, IConfiguration configuration)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }
        public async Task<Response<int>> Handle(DeleteProductCategoryByIdCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id);
            if (productCategory == null)
                return new Response<int>("Product Category not found.");

            var predicates = new List<Expression<Func<Product, bool>>>
            {
                p => p.ProductCategoryId == request.Id
            };

            var hasProduct = await _productRepository.FirstOrDefaultAsync(predicates);

            if (hasProduct != null)
                return new Response<int>("Product Category is still in use by one or more products.");

            if (!string.IsNullOrWhiteSpace(productCategory.ImageUrl))
            {
                await _fileService.DeleteFileAsync(productCategory.ImageUrl);
            }

            await _productCategoryRepository.DeleteAsync(productCategory);
            return new Response<int>(productCategory.Id);
        }
    }
}
