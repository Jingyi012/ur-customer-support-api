using Application.DTOs.Project;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<Response<ProjectResponseDto>>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Response<ProjectResponseDto>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IMapper _mapper;
        public GetProjectByIdQueryHandler(IProjectRepositoryAsync projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        public async Task<Response<ProjectResponseDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null) throw new ApiException($"Project Not Found.");

            var mappedProjects = _mapper.Map<ProjectResponseDto>(project);
            return new Response<ProjectResponseDto>(mappedProjects);
        }
    }
}
