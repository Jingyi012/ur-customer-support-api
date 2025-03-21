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
using Application.Features.Enquiry.Queries;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class EnquiryRepositoryAsync : GenericRepositoryAsync<Enquiry>, IEnquiryRepositoryAsync
    {
        private readonly DbSet<Enquiry> _enquiries;

        public EnquiryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _enquiries = dbContext.Set<Enquiry>();
        }

        public async Task<PagedResponse<List<Enquiry>>> GetAllEnquiry(GetAllEnquiryParameter filter)
        {
            var query = _enquiries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.CompanyName))
            {
                query = query.Where(u => u.CompanyName.Contains(filter.CompanyName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(u => u.CompanyName.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                query = query.Where(u => u.CompanyName.Contains(filter.Phone, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(u => u.Status == filter.Status);
            }

            if (filter.Type.HasValue)
            {
                query = query.Where(u => u.Type == filter.Type);
            }

            if (!string.IsNullOrWhiteSpace(filter.AssignedTo))
            {
                query = query.Where(u => u.CompanyName.Contains(filter.AssignedTo, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = await query.CountAsync();

            var product = await query
                .OrderByDescending(p => p.LastModified)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<Enquiry>>(product, filter.PageNumber, filter.PageSize, totalCount);
        }
    }
}
