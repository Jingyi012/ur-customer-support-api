using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public string Manufacturer { get; set; }
        public List<ProductImage> Images { get; set; } = [];
        public bool IsActive { get; set; } = true;
    }
}
