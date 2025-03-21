using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands
{
    public class CreateProjectCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<string>? Images { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Date)
                .GreaterThan(DateTime.MinValue).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("{PropertyName} must be between -90 and 90.");

            RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("{PropertyName} must be between -180 and 180.");
        }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Response<int>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IProjectRepositoryAsync projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Project>(request);

            var validImages = request.Images?
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Distinct()
                .ToList();

            if (validImages != null && validImages.Count > 0)
            {
                project.Images = validImages.Select((url, index) => new ProjectImage
                {
                    ImageUrl = url,
                    IsPrimary = index == 0
                }).ToList();
            }

            await _projectRepository.AddAsync(project);
            return new Response<int>(project.Id);
        }
    }
}
