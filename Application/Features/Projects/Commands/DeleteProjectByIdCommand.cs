using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands
{
    public class DeleteProjectByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteProjectByIdCommandHandler : IRequestHandler<DeleteProjectByIdCommand, Response<int>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;
        public DeleteProjectByIdCommandHandler(IProjectRepositoryAsync projectRepository, IFileService fileService, IConfiguration configuration)
        {
            _projectRepository = projectRepository; _fileService = fileService;
            _baseUrl = configuration["BaseUrl"] ?? string.Empty;
        }
        public async Task<Response<int>> Handle(DeleteProjectByIdCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null) throw new ApiException($"Project Not Found.");

            if (project.Images != null && project.Images.Count > 0)
            {
                foreach (var image in project.Images)
                {
                    if (!string.IsNullOrWhiteSpace(image.ImageUrl))
                    {
                        string imagePath = image.ImageUrl.StartsWith(_baseUrl)
                            ? image.ImageUrl.Substring(_baseUrl.Length)
                            : image.ImageUrl;

                        await _fileService.DeleteFileAsync(imagePath);
                    }
                }
            }

            await _projectRepository.DeleteAsync(project);
            return new Response<int>(project.Id);
        }
    }
}
