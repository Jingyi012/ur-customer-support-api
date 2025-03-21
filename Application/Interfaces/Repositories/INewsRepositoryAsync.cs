using Application.Features.News.Queries;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INewsRepositoryAsync : IGenericRepositoryAsync<News>
    {
        Task<PagedResponse<List<News>>> GetAllNews(GetAllNewsParameter filter);
    }
}
