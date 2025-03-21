using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Enquiry
{
    public class EnquiryWithHistoryResponseDto : EnquiryResponseDto
    {
        public List<EnquiryHistoryResponseDto> History { get; set; }
    }
}
