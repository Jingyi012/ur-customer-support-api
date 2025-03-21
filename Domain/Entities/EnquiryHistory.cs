using Domain.Common;

namespace Domain.Entities
{
    public class EnquiryHistory: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EnquiryType Type { get; set; }
        public string Message { get; set; }
        public EnquiryStatus Status { get; set; }
        public string AssignedTo { get; set; }
        public string? Remarks { get; set; } = string.Empty;
        public int EnquiryId { get; set; }
        public Enquiry Enquiry { get; set; }
    }
}
