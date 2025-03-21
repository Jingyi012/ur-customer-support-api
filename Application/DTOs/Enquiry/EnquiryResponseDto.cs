using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Enquiry
{
    public class EnquiryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EnquiryType Type { get; set; }
        public string Message { get; set; }
        public EnquiryStatus Status { get; set; }
        public string AssignedTo { get; set; }
        public string? Remarks { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
