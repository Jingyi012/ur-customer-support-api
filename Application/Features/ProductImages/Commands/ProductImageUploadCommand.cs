using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductImages.Commands
{
    public class ProductImageUploadCommand : IRequest<Response<List<string>>>
    {
        public int ProductId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }

    public class ProductImageUploadCommandHandler : IRequestHandler<ProductImageUploadCommand, Response<List<string>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IProductImageRepositoryAsync _productImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public ProductImageUploadCommandHandler(IProductRepositoryAsync productRepository, IProductImageRepositoryAsync productImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"];
        }

        public async Task<Response<List<string>>> Handle(ProductImageUploadCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new ApiException($"Product Not Found.");
            }

            int nextOrder = product.Images.Any() ? product.Images.Max(img => img.Order) + 1 : 1;

            var updatedImages = new List<ProductImage>();
            List<string> imageUrls = new List<string>();

            if (request.ImageFiles != null && request.ImageFiles.Any())
            {
                foreach (var file in request.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var savedPath = await _fileService.SaveFileAsync(file, product.Name, "img/product");
                        imageUrls.Add(_baseUrl + savedPath);

                        var productImage = new ProductImage
                        {
                            ProductId = request.ProductId,
                            ImageUrl = savedPath,
                            Order = nextOrder++
                        };

                        updatedImages.Add(productImage);
                    }
                }
            }

            if (updatedImages.Any())
            {
                await _productImageRepository.UpdateImagesAsync(updatedImages);
            }

            return new Response<List<string>>(imageUrls, "Images uploaded successfully.");
        }
    }
}
