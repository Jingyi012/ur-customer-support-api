using Application.Enums;
using Application.Filters;
namespace Application.Features.Projects.Queries
{
    public class GetAllProjectsParameter : RequestParameter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
