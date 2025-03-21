using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Dashboard
{
    public class DashboardStatisticDto
    {
        public int TotalProducts { get; set; }
        public int TotalNews { get; set; }
        public int TotalProjects { get; set; }
        public int TotalPendingEnquiries { get; set; }
    }
}
