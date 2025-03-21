using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Wrappers;
using System.Linq;
using Application.Features.News.Queries;

namespace Infrastructure.Persistence.Repositories
{
    public class NewsRepositoryAsync : GenericRepositoryAsync<News>, INewsRepositoryAsync
    {
        private readonly DbSet<News> _news;
        private readonly ApplicationDbContext _dbContext;

        public NewsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _news = dbContext.Set<News>();
            _dbContext = dbContext;
        }

        public override async Task UpdateAsync(News news)
        {
            var existingNewsImage = await _dbContext.NewsImages.Where(p => p.NewsId == news.Id).ToListAsync();
            _dbContext.NewsImages.RemoveRange(existingNewsImage);

            foreach (var image in news.Images)
            {
                _dbContext.NewsImages.Add(image);
            }

            _dbContext.Entry(news).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<News>>> GetAllNews(GetAllNewsParameter filter)
        {
            var query = _news.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(u => u.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase));
            }

            if(filter.Year.HasValue)
            {
                query = query.Where(u => u.Date.Year == filter.Year.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            var news = await query
                .OrderByDescending(p => p.Date)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<News>>(news, filter.PageNumber, filter.PageSize, totalCount);
        }
    }
}
