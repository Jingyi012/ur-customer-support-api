using Domain.Common;

namespace Domain.Entities
{
    public class ProductCategory : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
