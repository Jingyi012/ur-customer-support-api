using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.News.Commands
{
    public class UpdateNewsCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, Response<int>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IMapper _mapper;
        public UpdateNewsCommandHandler(INewsRepositoryAsync newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetByIdAsync(request.Id);

            if (news == null)
            {
                throw new ApiException($"News Not Found.");
            }
            else
            {
                _mapper.Map(request, news);

                await _newsRepository.UpdateAsync(news);
                return new Response<int>(news.Id);
            }
        }
    }
}
