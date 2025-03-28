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
    public class ProductImageRepositoryAsync : GenericRepositoryAsync<ProductImage>, IProductImageRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductImageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _dbContext.ProductImages
                .Where(img => img.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddImagesAsync(List<ProductImage> images)
        {
            if (images == null || !images.Any()) return;

            await _dbContext.ProductImages.AddRangeAsync(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveImagesAsync(List<ProductImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.ProductImages.RemoveRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImagesAsync(List<ProductImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.ProductImages.UpdateRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductImage> GetByUrlAsync(string imageUrl)
        {
            return await _dbContext.ProductImages.FirstOrDefaultAsync(img => img.ImageUrl == imageUrl);
        }

        public async Task<List<ProductImage>> GetByUrlsAsync(List<string> imageUrls)
        {
            return await _dbContext.ProductImages
                .Where(img => imageUrls.Contains(img.ImageUrl))
                .ToListAsync();
        }
    }

}
