using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductCategoryId { get; set; }
        public string Manufacturer { get; set; }
        public List<string> ImageUrls { get; set; }
        public bool IsActive { get; set; }
    }
}
