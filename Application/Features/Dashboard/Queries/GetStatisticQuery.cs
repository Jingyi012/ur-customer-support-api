using Application.DTOs.Dashboard;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Dashboard.Queries
{
    public class GetStatisticQuery : IRequest<Response<DashboardStatisticDto>>
    {
    }

    public class GetStatisticQueryHandler : IRequestHandler<GetStatisticQuery, Response<DashboardStatisticDto>>
    {
        private readonly IProjectRepositoryAsync _projectRepository;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly INewsRepositoryAsync _newsRepository;
        private readonly IEnquiryRepositoryAsync _enquiryRepository;

        public GetStatisticQueryHandler(IProjectRepositoryAsync projectRepository, IProductRepositoryAsync productRepository, INewsRepositoryAsync newsRepository, IEnquiryRepositoryAsync enquiryRepository)
        {
            _projectRepository = projectRepository;
            _productRepository = productRepository;
            _newsRepository = newsRepository;
            _enquiryRepository = enquiryRepository;
        }

        public async Task<Response<DashboardStatisticDto>> Handle(GetStatisticQuery request, CancellationToken cancellationToken)
        {
            var totalProjects = await _projectRepository.CountAsync();
            var totalProducts = await _productRepository.CountAsync();
            var totalNews = await _newsRepository.CountAsync();
            var totalPendingEnquiries = await _enquiryRepository.CountAsync(x => x.Status == EnquiryStatus.Pending || x.Status == EnquiryStatus.InProgress);

            var result = new DashboardStatisticDto
            {
                TotalProjects = totalProjects,
                TotalProducts = totalProducts,
                TotalNews = totalNews,
                TotalPendingEnquiries = totalPendingEnquiries
            };
            return new Response<DashboardStatisticDto>(result);
        }
    }
}
