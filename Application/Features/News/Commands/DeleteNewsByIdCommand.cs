using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.News.Commands
{
    public class DeleteNewsByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteNewsByIdCommandHandler : IRequestHandler<DeleteNewsByIdCommand, Response<int>>
    {
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IFileService _fileService;
        private readonly string _baseUrl;

        public DeleteNewsByIdCommandHandler(INewsRepositoryAsync newsRepository, IFileService fileService, IConfiguration configuration)
        {
            _newsRepository = newsRepository;
            _fileService = fileService;
            _baseUrl = configuration["BaseUrl"];
        }
        public async Task<Response<int>> Handle(DeleteNewsByIdCommand request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetByIdAsync(request.Id);
            if (news == null) throw new ApiException($"News Not Found.");

            if (news.Images != null && news.Images.Count > 0)
            {
                foreach (var image in news.Images)
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

            await _newsRepository.DeleteAsync(news);
            return new Response<int>(news.Id);
        }
    }
}
