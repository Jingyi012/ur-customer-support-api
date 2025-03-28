using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.News.Commands
{
    public partial class CreateNewsCommand : IRequest<Response<int>>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
    {
        public CreateNewsCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Date)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }

    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, Response<int>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IMapper _mapper;

        public CreateNewsCommandHandler(INewsRepositoryAsync newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<Domain.Entities.News>(request);

            await _newsRepository.AddAsync(news);
            return new Response<int>(news.Id);
        }
    }
}
