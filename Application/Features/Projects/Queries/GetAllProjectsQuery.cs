using Application.DTOs.Project;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Projects.Queries
{
    public class GetAllProjectsQuery : IRequest<object>
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; } = false;
    }

    public class AdminGetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, object>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IMapper _mapper;
        public AdminGetAllProjectsQueryHandler(IProjectRepositoryAsync projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            if ((bool)request.IsAdmin)
            {
                var validFilter = _mapper.Map<GetAllProjectsParameter>(request);
                var projects = await _projectRepository.GetAllProjects(validFilter);
                var mappedProjects = _mapper.Map<List<ProjectResponseDto>>(projects.Data);

                var projectsResponse = new PagedResponse<List<ProjectResponseDto>>(
                    mappedProjects,
                    projects.PageNumber,
                    projects.PageSize,
                    projects.TotalCount
                );

                return projectsResponse;
            }
            else
            {
                var projects = await _projectRepository.GetAllAsync(new List<Expression<Func<Project, object>>> { projects => projects.Images });
                var mappedProjects = _mapper.Map<List<ProjectResponseDto>>(projects);

                return new Response<List<ProjectResponseDto>>(mappedProjects);
            }

        }
    }
}
