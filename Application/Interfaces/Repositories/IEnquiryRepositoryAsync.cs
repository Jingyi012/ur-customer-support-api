using Application.Features.Enquiry.Queries;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IEnquiryRepositoryAsync : IGenericRepositoryAsync<Enquiry>
    {
        Task<PagedResponse<List<Enquiry>>> GetAllEnquiry(GetAllEnquiryParameter filter);
    }
}
