using Application.Enums;
using Application.Filters;
namespace Application.Features.News.Queries
{
    public class GetAllNewsParameter : RequestParameter
    {
        public string? Title { get; set; }
        public int? Year { get; set; }
        public bool? IsActive { get; set; }
    }
}
