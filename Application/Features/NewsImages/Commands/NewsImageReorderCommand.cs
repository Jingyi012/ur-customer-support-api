using Application.DTOs.NewsImage;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.NewsImages.Commands
{
    public class NewsImageReorderCommand : IRequest<Response<bool>>
    {
        public int NewsId { get; set; }
        public List<NewsImageOrderDto> Images { get; set; } = new();
    }

    public class NewsImageReorderCommandHandler : IRequestHandler<NewsImageReorderCommand, Response<bool>>
    {
        private readonly INewsImageRepositoryAsync _newsImageRepository;
        private readonly string _baseUrl;

        public NewsImageReorderCommandHandler(INewsImageRepositoryAsync newsImageRepository, IConfiguration configuration)
        {
            _newsImageRepository = newsImageRepository;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(NewsImageReorderCommand request, CancellationToken cancellationToken)
        {
            var existingImages = await _newsImageRepository.GetImagesByNewsIdAsync(request.NewsId);

            if (existingImages == null || !existingImages.Any())
            {
                throw new ApiException("No images found for the news.");
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

            await _newsImageRepository.UpdateImagesAsync(existingImages);

            return new Response<bool>(true, "Images reordered successfully.");
        }
    }
}
