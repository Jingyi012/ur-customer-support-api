using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Wrappers;
using Application.Features.Products.Queries;
using System.Linq;
using Application.Enums;
using Application.Exceptions;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;
        private readonly ApplicationDbContext _dbContext;

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Product>();
            _dbContext = dbContext;
        }

        public override async Task UpdateAsync(Product product)
        {
            var existingProductImage = await _dbContext.ProductImages.Where(p => p.ProductId == product.Id).ToListAsync();
            _dbContext.ProductImages.RemoveRange(existingProductImage);

            foreach (var image in product.Images)
            {
                _dbContext.ProductImages.Add(image);
            }

            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _products
                .Include(p => p.Images.OrderBy(img => !img.IsPrimary).ThenBy(img => img.Id))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<List<Product>>> GetAllProducts(GetAllProductsParameter filter)
        {
            var query = _products
                .Include(p => p.Images.OrderBy(img => !img.IsPrimary).ThenBy(img => img.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Manufacturer))
            {
                query = query.Where(u => u.Manufacturer.Contains(filter.Manufacturer, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.ProductCategoryId.HasValue)
            {
                query = query.Where(u => u.ProductCategoryId == filter.ProductCategoryId);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, filter.SortBy);

            var product = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<Product>>(product, filter.PageNumber, filter.PageSize, totalCount);
        }

        public static IQueryable<Product> ApplySorting(IQueryable<Product> query, SortByOption? sortBy)
        {
            return sortBy switch
            {
                SortByOption.NameLowToHigh => query.OrderBy(p => p.Name),
                SortByOption.NameHighToLow => query.OrderByDescending(p => p.Name),
                _ => query.OrderByDescending(p => p.Created)
            };
        }
    }
}
