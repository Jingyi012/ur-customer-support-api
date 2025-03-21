using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductCategoryRepositoryAsync : GenericRepositoryAsync<ProductCategory>, IProductCategoryRepositoryAsync
    {
        private readonly DbSet<ProductCategory> _category;

        public ProductCategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _category = dbContext.Set<ProductCategory>();
        }
    }
}
