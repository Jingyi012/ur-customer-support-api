using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProjectImageRepositoryAsync : IGenericRepositoryAsync<ProjectImage>
    {
        Task<List<ProjectImage>> GetImagesByProjectIdAsync(int projectId);
        Task AddImagesAsync(List<ProjectImage> images);
        Task RemoveImagesAsync(List<ProjectImage> images);
        Task UpdateImagesAsync(List<ProjectImage> images);
        Task<ProjectImage> GetByUrlAsync(string imageUrls);
        Task<List<ProjectImage>> GetByUrlsAsync(List<string> imageUrls);
    }

}
