using Application.Enums;
using Application.Filters;
namespace Application.Features.Products.Queries
{
    public class GetAllProductsParameter : RequestParameter
    {
        public string? Name { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public bool? IsActive { get; set; }
        public SortByOption? SortBy { get; set; }
    }
}
