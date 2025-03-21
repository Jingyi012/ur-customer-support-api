using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum EnquiryStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }

    public enum EnquiryType
    {
        ProductEnquiry,
        ConsultancySiteSurvey,
        Design,
        RepairMaintenance,
        Other
    }
}
