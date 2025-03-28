using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProjectImages.Commands
{
    public class ProjectImageUploadCommand : IRequest<Response<List<string>>>
    {
        public int ProjectId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }

    public class ProjectImageUploadCommandHandler : IRequestHandler<ProjectImageUploadCommand, Response<List<string>>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IProjectImageRepositoryAsync _projectImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public ProjectImageUploadCommandHandler(IProjectRepositoryAsync projectRepository, IProjectImageRepositoryAsync projectImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _projectRepository = projectRepository;
            _projectImageRepository = projectImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"];
        }

        public async Task<Response<List<string>>> Handle(ProjectImageUploadCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
            {
                throw new ApiException($"Project Not Found.");
            }

            int nextOrder = project.Images.Any() ? project.Images.Max(img => img.Order) + 1 : 1;

            var updatedImages = new List<ProjectImage>();
            List<string> imageUrls = new List<string>();

            if (request.ImageFiles != null && request.ImageFiles.Any())
            {
                foreach (var file in request.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var savedPath = await _fileService.SaveFileAsync(file, project.Name, "img/project");
                        imageUrls.Add(_baseUrl + savedPath);

                        var projectImage = new ProjectImage
                        {
                            ProjectId = request.ProjectId,
                            ImageUrl = savedPath,
                            Order = nextOrder++
                        };

                        updatedImages.Add(projectImage);
                    }
                }
            }

            if (updatedImages.Any())
            {
                await _projectImageRepository.UpdateImagesAsync(updatedImages);
            }

            return new Response<List<string>>(imageUrls, "Images uploaded successfully.");
        }
    }
}
