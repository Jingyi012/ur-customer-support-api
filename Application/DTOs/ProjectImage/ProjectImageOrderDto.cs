using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProjectImage
{
    public class ProjectImageOrderDto
    {
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
