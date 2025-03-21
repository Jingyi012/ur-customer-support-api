using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.News.Commands
{
    public class DeleteNewsByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteNewsByIdCommandHandler : IRequestHandler<DeleteNewsByIdCommand, Response<int>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        public DeleteNewsByIdCommandHandler(INewsRepositoryAsync newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<Response<int>> Handle(DeleteNewsByIdCommand request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetByIdAsync(request.Id);
            if (news == null) throw new ApiException($"News Not Found.");
            await _newsRepository.DeleteAsync(news);
            return new Response<int>(news.Id);
        }
    }
}
