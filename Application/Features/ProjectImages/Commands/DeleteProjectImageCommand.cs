using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProjectImages.Commands
{
    public class DeleteProjectImageCommand : IRequest<Response<bool>>
    {
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class DeleteProjectImageCommandHandler : IRequestHandler<DeleteProjectImageCommand, Response<bool>>
    {
        private readonly IProjectImageRepositoryAsync _projectImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public DeleteProjectImageCommandHandler(IProjectImageRepositoryAsync projectImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _projectImageRepository = projectImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(DeleteProjectImageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return new Response<bool>(false, "No image URL provided for deletion.");
            }

            string imagePath = request.ImageUrl.StartsWith(_baseUrl)
                ? request.ImageUrl.Substring(_baseUrl.Length)
                : request.ImageUrl;

            // Fetch the image by path
            var image = await _projectImageRepository.GetByUrlAsync(imagePath);

            if (image == null)
            {
                throw new ApiException("No matching image found for the provided URL.");
            }

            // Delete file from storage
            await _fileService.DeleteFileAsync(image.ImageUrl);

            // Delete image record from database
            await _projectImageRepository.DeleteAsync(image);

            return new Response<bool>(true, "Image deleted successfully.");
        }
    }
}
