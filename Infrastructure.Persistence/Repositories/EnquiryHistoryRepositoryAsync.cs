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
using System.Linq;
using Application.Enums;

namespace Infrastructure.Persistence.Repositories
{
    public class EnquiryHistoryRepositoryAsync : GenericRepositoryAsync<EnquiryHistory>, IEnquiryHistoryRepositoryAsync
    {
        private readonly DbSet<EnquiryHistory> _enquiryHistory;

        public EnquiryHistoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _enquiryHistory = dbContext.Set<EnquiryHistory>();
        }

        public async Task<List<EnquiryHistory>> GetEnquiryHistoryAsync(int enquiryId)
        {
            var enquiryHistory = await _enquiryHistory
                .Where(e => e.EnquiryId == enquiryId)
                .OrderByDescending(p => p.LastModified)
                .ToListAsync();

            return new List<EnquiryHistory>(enquiryHistory);
        }

    }
}
