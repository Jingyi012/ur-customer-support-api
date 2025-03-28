using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductImages.Queries
{
    public class GetImagesByCategoryAndItemIdQueries : IRequest<Response<List<string>>>
    {
        public int ItemId { get; set; }
        public string Category { get; set; }
    }

    public class GetImagesByProductIdQueriesHandler : IRequestHandler<GetImagesByCategoryAndItemIdQueries, Response<List<string>>>
    {
        private readonly IProductImageRepositoryAsync _productImageRepository;
        private readonly IProjectImageRepositoryAsync _projectImageRepository;
        private readonly INewsImageRepositoryAsync _newsImageRepository;
        private readonly string _baseUrl;

        public GetImagesByProductIdQueriesHandler(IProductImageRepositoryAsync productImageRepository, IProjectImageRepositoryAsync projectImageRepository, INewsImageRepositoryAsync newsImageRepository, IConfiguration configuration)
        {
            _productImageRepository = productImageRepository;
            _projectImageRepository = projectImageRepository;
            _newsImageRepository = newsImageRepository;
            _baseUrl = configuration["BaseUrl"];
        }

        public async Task<Response<List<string>>> Handle(GetImagesByCategoryAndItemIdQueries request, CancellationToken cancellationToken)
        {
            List<string> imageUrls = new();

            switch (request.Category.ToLower())
            {
                case "product":
                    var productImages = await _productImageRepository.GetImagesByProductIdAsync(request.ItemId);
                    imageUrls = productImages.Select(x => _baseUrl + x.ImageUrl).ToList();
                    break;

                case "project":
                    var projectImages = await _projectImageRepository.GetImagesByProjectIdAsync(request.ItemId);
                    imageUrls = projectImages.Select(x => _baseUrl + x.ImageUrl).ToList();
                    break;

                case "news":
                    var newsImages = await _newsImageRepository.GetImagesByNewsIdAsync(request.ItemId);
                    imageUrls = newsImages.Select(x => _baseUrl + x.ImageUrl).ToList();
                    break;

                default:
                    return new Response<List<string>>("Invalid category specified");
            }

            return new Response<List<string>>(imageUrls);
        }
    }
}
