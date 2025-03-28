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

namespace Application.Features.NewsImages.Commands
{
    public class NewsImageUploadCommand : IRequest<Response<List<string>>>
    {
        public int NewsId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }

    public class NewsImageUploadCommandHandler : IRequestHandler<NewsImageUploadCommand, Response<List<string>>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly INewsImageRepositoryAsync _newsImageRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public NewsImageUploadCommandHandler(INewsRepositoryAsync newsRepository, INewsImageRepositoryAsync newsImageRepository, IFileService fileService, IConfiguration configuration)
        {
            _newsRepository = newsRepository;
            _newsImageRepository = newsImageRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"];
        }

        public async Task<Response<List<string>>> Handle(NewsImageUploadCommand request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetByIdAsync(request.NewsId);

            if (news == null)
            {
                throw new ApiException($"News Not Found.");
            }

            int nextOrder = news.Images.Any() ? news.Images.Max(img => img.Order) + 1 : 1;

            var updatedImages = new List<NewsImage>();
            List<string> imageUrls = new List<string>();

            if (request.ImageFiles != null && request.ImageFiles.Any())
            {
                foreach (var file in request.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var savedPath = await _fileService.SaveFileAsync(file, news.Title, "img/news");
                        imageUrls.Add(_baseUrl + savedPath);

                        var newsImage = new NewsImage
                        {
                            NewsId = request.NewsId,
                            ImageUrl = savedPath,
                            Order = nextOrder++
                        };

                        updatedImages.Add(newsImage);
                    }
                }
            }

            if (updatedImages.Any())
            {
                await _newsImageRepository.UpdateImagesAsync(updatedImages);
            }

            return new Response<List<string>>(imageUrls, "Images uploaded successfully.");
        }
    }
}
