using Application.DTOs.News;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.News.Queries
{
    public class GetNewsByIdQuery : IRequest<Response<NewsResponseDto>>
    {
        public int Id { get; set; }
    }

    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, Response<NewsResponseDto>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IMapper _mapper;
        public GetNewsByIdQueryHandler(INewsRepositoryAsync newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task<Response<NewsResponseDto>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetByIdAsync(request.Id);
            if (news == null) throw new ApiException($"News Not Found.");

            var mappedNews = _mapper.Map<NewsResponseDto>(news);
            return new Response<NewsResponseDto>(mappedNews);
        }
    }
}
