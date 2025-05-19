using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Commands
{
    public class UpdateProductCategoryCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Response<int>>
    {
        private readonly IProductCategoryRepositoryAsync _productCategoryRepository;
        private readonly IMapper _mapper;
        private ILogger<UpdateProductCategoryCommandHandler> _logger;
        private readonly IFileService _fileService;

        public UpdateProductCategoryCommandHandler(IProductCategoryRepositoryAsync productCategoryRepository, IMapper mapper, ILogger<UpdateProductCategoryCommandHandler> logger,
            IFileService fileService)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<Response<int>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id);

            if (productCategory == null)
                throw new ApiException("Product category not found.");

            if (!string.Equals(productCategory.Name, request.Name, StringComparison.Ordinal))
            {
                productCategory.Name = request.Name;
            }

            if (request.ImageFile != null)
            {
                if (!string.IsNullOrWhiteSpace(productCategory.ImageUrl))
                {
                    await _fileService.DeleteFileAsync(productCategory.ImageUrl);
                }

                var savedPath = await _fileService.SaveFileAsync(request.ImageFile, request.Name, "img/productCategory");
                productCategory.ImageUrl = savedPath;
            }

            await _productCategoryRepository.UpdateAsync(productCategory);

            return new Response<int>(productCategory.Id);
        }
    }
}