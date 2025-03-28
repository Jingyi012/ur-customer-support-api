using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INewsImageRepositoryAsync : IGenericRepositoryAsync<NewsImage>
    {
        Task<List<NewsImage>> GetImagesByNewsIdAsync(int newsId);
        Task AddImagesAsync(List<NewsImage> images);
        Task RemoveImagesAsync(List<NewsImage> images);
        Task UpdateImagesAsync(List<NewsImage> images);
        Task<NewsImage> GetByUrlAsync(string imageUrls);
        Task<List<NewsImage>> GetByUrlsAsync(List<string> imageUrls);
    }

}
