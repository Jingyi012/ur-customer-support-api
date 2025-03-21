using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
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
        public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Response<int>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) throw new ApiException($"Product Not Found.");
            await _productRepository.DeleteAsync(product);
            return new Response<int>(product.Id);
        }
    }
}
