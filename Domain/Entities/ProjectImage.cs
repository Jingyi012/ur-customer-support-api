﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectImage : AuditableBaseEntity
    {
        public int ProjectId { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public Project Project { get; set; }
    }
}
