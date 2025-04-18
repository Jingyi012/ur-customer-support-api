﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage : AuditableBaseEntity
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public int Order {  get; set; }
        public Product Product { get; set; }
    }
}
