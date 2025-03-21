using Application.Features.Products.Queries;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<PagedResponse<List<Product>>> GetAllProducts(GetAllProductsParameter filter);
    }
}
