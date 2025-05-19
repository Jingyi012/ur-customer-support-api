using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Features.ProductCategories.Commands
{
    public class CreateProductCategoryCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class CreateProductCategoryCommandHandler
        : IRequestHandler<CreateProductCategoryCommand, Response<int>>
    {
        private readonly IProductCategoryRepositoryAsync _productCategoryRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public CreateProductCategoryCommandHandler(
            IProductCategoryRepositoryAsync productCategoryRepository,
            IFileService fileService,
            IConfiguration configuration
        )
        {
            _productCategoryRepository = productCategoryRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"];
        }

        public async Task<Response<int>> Handle(
            CreateProductCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            string imageUrl = "";

            if (request.ImageFile != null)
            {
                var file = request.ImageFile;
                if (file.Length > 0)
                {
                    var savedPath = await _fileService.SaveFileAsync(
                        file,
                        request.Name,
                        "img/productCategory"
                    );
                    imageUrl = savedPath;
                }
            }

            var productCategory = new ProductCategory { Name = request.Name, ImageUrl = imageUrl };

            var response = await _productCategoryRepository.AddAsync(productCategory);

            return new Response<int>(response.Id, "Product category created successfully.");
        }
    }
}
