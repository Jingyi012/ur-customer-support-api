using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProductImageRepositoryAsync : IGenericRepositoryAsync<ProductImage>
    {
        Task<List<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task AddImagesAsync(List<ProductImage> images);
        Task RemoveImagesAsync(List<ProductImage> images);
        Task UpdateImagesAsync(List<ProductImage> images);
        Task<ProductImage> GetByUrlAsync(string imageUrls);
        Task<List<ProductImage>> GetByUrlsAsync(List<string> imageUrls);
    }

}
