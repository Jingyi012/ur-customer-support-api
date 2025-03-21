using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Enquiry: AuditableBaseEntity
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
        public List<EnquiryHistory> History { get; set; } = new();

        public void UpdateHistory()
        {
            EnquiryHistory history = new()
            {
                EnquiryId = Id,
                Name = Name,
                CompanyName = CompanyName,
                Email = Email,
                Phone = Phone,
                Type = Type,
                Message = Message,
                Status = Status,
                AssignedTo = AssignedTo,
                Remarks = Remarks
            };

            History.Add(history);
        }
    }
}
