using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class News : AuditableBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<NewsImage> Images { get; set; } = [];
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
