using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.News
{
    public class NewsResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
