using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<string> ImageUrls { get; set; }
        public bool IsActive { get; set; }
    }
}
