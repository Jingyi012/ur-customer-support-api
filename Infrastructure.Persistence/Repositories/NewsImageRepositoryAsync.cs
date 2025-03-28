using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class NewsImageRepositoryAsync : GenericRepositoryAsync<NewsImage>, INewsImageRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsImageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<NewsImage>> GetImagesByNewsIdAsync(int newsId)
        {
            return await _dbContext.NewsImages
                .Where(img => img.NewsId == newsId)
                .ToListAsync();
        }

        public async Task AddImagesAsync(List<NewsImage> images)
        {
            if (images == null || !images.Any()) return;

            await _dbContext.NewsImages.AddRangeAsync(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveImagesAsync(List<NewsImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.NewsImages.RemoveRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImagesAsync(List<NewsImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.NewsImages.UpdateRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<NewsImage> GetByUrlAsync(string imageUrl)
        {
            return await _dbContext.NewsImages.FirstOrDefaultAsync(img => img.ImageUrl == imageUrl);
        }

        public async Task<List<NewsImage>> GetByUrlsAsync(List<string> imageUrls)
        {
            return await _dbContext.NewsImages
                .Where(img => imageUrls.Contains(img.ImageUrl))
                .ToListAsync();
        }
    }

}
