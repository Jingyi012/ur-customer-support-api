using Application.Enums;
using Application.Filters;
using Domain.Entities;
namespace Application.Features.Enquiry.Queries
{
    public class GetAllEnquiryParameter : RequestParameter
    {
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public EnquiryType? Type { get; set; }
        public EnquiryStatus? Status { get; set; }
        public string? AssignedTo { get; set; }
    }
}
