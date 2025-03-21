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

namespace Application.Features.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<string>? Images { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Response<int>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IMapper _mapper;
        public UpdateProjectCommandHandler(IProjectRepositoryAsync projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);

            if (project == null)
            {
                throw new ApiException($"Project Not Found.");
            }
            else
            {
                _mapper.Map(request, project);
                UpdateProjectImages(project, request.Images);
                await _projectRepository.UpdateAsync(project);
                return new Response<int>(project.Id);
            }
        }

        private void UpdateProjectImages(Project project, List<string>? imageUrls)
        {
            if (imageUrls == null)
            {
                return;
            }

            var updatedImages = imageUrls
                .Select((url, index) => new ProjectImage
                {
                    ProjectId = project.Id,
                    ImageUrl = url,
                    IsPrimary = (index == 0)
                })
                .ToList();

            project.Images.Clear();

            project.Images.AddRange(updatedImages);
        }
    }
}
