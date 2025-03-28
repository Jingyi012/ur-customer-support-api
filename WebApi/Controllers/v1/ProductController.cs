using System.Threading.Tasks;
using Application.Features.ProductImages.Commands;
using Application.Features.ProductImages.Queries;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQuery query)
        {
          
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateProductCommand command)
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
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        // GET api/<controller>/5/images
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImagesById(int id)
        {
            return Ok(await Mediator.Send(new GetImagesByCategoryAndItemIdQueries { ItemId = id, Category = "Product" }));
        }

        // POST api/<controller>
        [HttpPost("images")]
        [Authorize]
        public async Task<IActionResult> AddImages([FromForm] ProductImageUploadCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("images/reorder")]
        [Authorize]
        public async Task<IActionResult> ReorderImages(ProductImageReorderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("images")]
        [Authorize]
        public async Task<IActionResult> DeleteImages([FromQuery] DeleteProductImageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
