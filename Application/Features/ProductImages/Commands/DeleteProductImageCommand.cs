using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductImages.Commands
{
    public class DeleteProductImageCommand : IRequest<Response<bool>>
    {
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, Response<bool>>
    {
        private readonly IProductImageRepositoryAsync _productImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public DeleteProductImageCommandHandler(IProductImageRepositoryAsync productImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _productImageRepository = productImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public async Task<Response<bool>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return new Response<bool>(false, "No image URL provided for deletion.");
            }

            string imagePath = request.ImageUrl.StartsWith(_baseUrl)
                ? request.ImageUrl.Substring(_baseUrl.Length)
                : request.ImageUrl;

            // Fetch the image by path
            var image = await _productImageRepository.GetByUrlAsync(imagePath);

            if (image == null)
            {
                throw new ApiException("No matching image found for the provided URL.");
            }

            // Delete file from storage
            await _fileService.DeleteFileAsync(image.ImageUrl);

            // Delete image record from database
            await _productImageRepository.DeleteAsync(image);

            return new Response<bool>(true, "Image deleted successfully.");
        }
    }
}
