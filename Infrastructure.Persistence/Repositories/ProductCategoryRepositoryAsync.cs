using Application.Features.News.Queries;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductCategoryRepositoryAsync : GenericRepositoryAsync<ProductCategory>, IProductCategoryRepositoryAsync
    {
        private readonly DbSet<ProductCategory> _category;

        public ProductCategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _category = dbContext.Set<ProductCategory>();
        }

        public async Task<PagedResponse<List<ProductCategory>>> GetAllProductCategory(int pageNumber, int pageSize)
        {
            var query = _category.AsQueryable();

            var totalCount = await query.CountAsync();

            var productCategories = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<ProductCategory>>(productCategories, pageNumber, pageSize, totalCount);
        }
    }
}
