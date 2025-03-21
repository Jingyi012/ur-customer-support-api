using Application.DTOs.News;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.News.Queries
{
    public class GetAllNewsQuery : IRequest<object>
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
        public string? Title { get; set; }
        public int? Year { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AdminGetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, object>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IMapper _mapper;
        public AdminGetAllNewsQueryHandler(INewsRepositoryAsync newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllNewsParameter>(request);
            var news = await _newsRepository.GetAllNews(validFilter);
            var mappedNews = _mapper.Map<List<NewsResponseDto>>(news.Data);

            var newsResponse = new PagedResponse<List<NewsResponseDto>>(
                mappedNews,
                news.PageNumber,
                news.PageSize,
                news.TotalCount
            );

            return newsResponse;
        }
    }
}
