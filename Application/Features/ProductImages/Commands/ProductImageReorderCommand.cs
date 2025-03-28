using Application.DTOs.ProductImage;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductImages.Commands
{
    public class ProductImageReorderCommand : IRequest<Response<bool>>
    {
        public int ProductId { get; set; }
        public List<ProductImageOrderDto> Images { get; set; } = new();
    }

    public class ProductImageReorderCommandHandler : IRequestHandler<ProductImageReorderCommand, Response<bool>>
    {
        private readonly IProductImageRepositoryAsync _productImageRepository;
        private readonly string _baseUrl;

        public ProductImageReorderCommandHandler(IProductImageRepositoryAsync productImageRepository, IConfiguration configuration)
        {
            _productImageRepository = productImageRepository;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(ProductImageReorderCommand request, CancellationToken cancellationToken)
        {
            var existingImages = await _productImageRepository.GetImagesByProductIdAsync(request.ProductId);

            if (existingImages == null || !existingImages.Any())
            {
                throw new ApiException("No images found for the product.");
            }

            var imageDict = existingImages.ToDictionary(img => img.ImageUrl);

            foreach (var imgDto in request.Images)
            {
                string imagePath = imgDto.ImageUrl.StartsWith(_baseUrl)
                ? imgDto.ImageUrl.Substring(_baseUrl.Length)
                : imgDto.ImageUrl;

                if (imageDict.TryGetValue(imagePath, out var image))
                {
                    image.Order = imgDto.Order;
                }
                else
                {
                    throw new ApiException($"Image with url {imgDto.ImageUrl} not found.");
                }
            }

            await _productImageRepository.UpdateImagesAsync(existingImages);

            return new Response<bool>(true, "Images reordered successfully.");
        }
    }
}
