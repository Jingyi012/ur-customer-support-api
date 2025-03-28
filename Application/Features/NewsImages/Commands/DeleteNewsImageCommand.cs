using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.NewsImages.Commands
{
    public class DeleteNewsImageCommand : IRequest<Response<bool>>
    {
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class DeleteNewsImageCommandHandler : IRequestHandler<DeleteNewsImageCommand, Response<bool>>
    {
        private readonly INewsImageRepositoryAsync _newsImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public DeleteNewsImageCommandHandler(INewsImageRepositoryAsync newsImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _newsImageRepository = newsImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(DeleteNewsImageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return new Response<bool>(false, "No image URL provided for deletion.");
            }

            string imagePath = request.ImageUrl.StartsWith(_baseUrl)
                ? request.ImageUrl.Substring(_baseUrl.Length)
                : request.ImageUrl;

            // Fetch the image by path
            var image = await _newsImageRepository.GetByUrlAsync(imagePath);

            if (image == null)
            {
                throw new ApiException("No matching image found for the provided URL.");
            }

            // Delete file from storage
            await _fileService.DeleteFileAsync(image.ImageUrl);

            // Delete image record from database
            await _newsImageRepository.DeleteAsync(image);

            return new Response<bool>(true, "Image deleted successfully.");
        }
    }
}
