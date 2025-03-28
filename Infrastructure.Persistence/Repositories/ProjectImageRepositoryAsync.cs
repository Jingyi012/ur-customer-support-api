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
    public class ProjectImageRepositoryAsync : GenericRepositoryAsync<ProjectImage>, IProjectImageRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectImageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProjectImage>> GetImagesByProjectIdAsync(int projectId)
        {
            return await _dbContext.ProjectImages
                .Where(img => img.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddImagesAsync(List<ProjectImage> images)
        {
            if (images == null || !images.Any()) return;

            await _dbContext.ProjectImages.AddRangeAsync(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveImagesAsync(List<ProjectImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.ProjectImages.RemoveRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImagesAsync(List<ProjectImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.ProjectImages.UpdateRange(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProjectImage> GetByUrlAsync(string imageUrl)
        {
            return await _dbContext.ProjectImages.FirstOrDefaultAsync(img => img.ImageUrl == imageUrl);
        }

        public async Task<List<ProjectImage>> GetByUrlsAsync(List<string> imageUrls)
        {
            return await _dbContext.ProjectImages
                .Where(img => imageUrls.Contains(img.ImageUrl))
                .ToListAsync();
        }
    }

}
