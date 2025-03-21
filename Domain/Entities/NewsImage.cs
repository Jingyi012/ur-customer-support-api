using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NewsImage : AuditableBaseEntity
    {
        public int NewsId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } = false;
        public News News { get; set; }
    }
}
