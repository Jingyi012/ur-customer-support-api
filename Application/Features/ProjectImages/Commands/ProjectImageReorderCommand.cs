using Application.DTOs.ProjectImage;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProjectImages.Commands
{
    public class ProjectImageReorderCommand : IRequest<Response<bool>>
    {
        public int ProjectId { get; set; }
        public List<ProjectImageOrderDto> Images { get; set; } = new();
    }

    public class ProjectImageReorderCommandHandler : IRequestHandler<ProjectImageReorderCommand, Response<bool>>
    {
        private readonly IProjectImageRepositoryAsync _projectImageRepository;
        private readonly string _baseUrl;

        public ProjectImageReorderCommandHandler(IProjectImageRepositoryAsync projectImageRepository, IConfiguration configuration)
        {
            _projectImageRepository = projectImageRepository;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(ProjectImageReorderCommand request, CancellationToken cancellationToken)
        {
            var existingImages = await _projectImageRepository.GetImagesByProjectIdAsync(request.ProjectId);

            if (existingImages == null || !existingImages.Any())
            {
                throw new ApiException("No images found for the project.");
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

            await _projectImageRepository.UpdateImagesAsync(existingImages);

            return new Response<bool>(true, "Images reordered successfully.");
        }
    }
}
