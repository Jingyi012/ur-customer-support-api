using System.Threading.Tasks;
using Application.Features.Dashboard.Queries;
using Application.Features.Products.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DashboardController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetStatisticQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
