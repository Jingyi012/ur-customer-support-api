using System.Threading.Tasks;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductImages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductCategoryController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductCategoryQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProductCategory([FromForm] CreateProductCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProductCategory(int id, [FromForm] UpdateProductCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductCategoryByIdCommand { Id = id }));
        }
    }
}
