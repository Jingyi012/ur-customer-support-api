using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            
            #region Repositories
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddScoped<IProductCategoryRepositoryAsync, ProductCategoryRepositoryAsync>();
            services.AddScoped<IProjectRepositoryAsync, ProjectRepositoryAsync>();
            services.AddScoped<INewsRepositoryAsync, NewsRepositoryAsync>();
            services.AddScoped<IEnquiryRepositoryAsync, EnquiryRepositoryAsync>();
            services.AddScoped<IEnquiryHistoryRepositoryAsync, EnquiryHistoryRepositoryAsync>();
            #endregion
        }
    }
}
