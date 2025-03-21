using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Project : AuditableBaseEntity 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<ProjectImage> Images { get; set; } = [];
        public bool IsActive { get; set; }
    }
}
