using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IFileService _fileService; 
        private readonly string _baseUrl;
        public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepository, IFileService fileService, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }
        public async Task<Response<int>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) throw new ApiException($"Product Not Found.");

            if (product.Images != null && product.Images.Count > 0)
            {
                foreach (var image in product.Images)
                {
                    if (!string.IsNullOrWhiteSpace(image.ImageUrl))
                    {
                        string imagePath = image.ImageUrl.StartsWith(_baseUrl)
                            ? image.ImageUrl.Substring(_baseUrl.Length)
                            : image.ImageUrl;

                        await _fileService.DeleteFileAsync(imagePath);
                    }
                }
            }

            await _productRepository.DeleteAsync(product);
            return new Response<int>(product.Id);
        }
    }
}
