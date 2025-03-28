using System.Threading.Tasks;
using Application.Features.News.Commands;
using Application.Features.News.Queries;
using Application.Features.ProductImages.Queries;
using Application.Features.NewsImages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class NewsController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllNewsQuery query)
        {
          
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetNewsByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateNewsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateNewsCommand command)
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
            return Ok(await Mediator.Send(new DeleteNewsByIdCommand { Id = id }));
        }

        // GET api/<controller>/5/images
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImagesById(int id)
        {
            return Ok(await Mediator.Send(new GetImagesByCategoryAndItemIdQueries { ItemId = id, Category = "News" }));
        }

        // POST api/<controller>
        [HttpPost("images")]
        [Authorize]
        public async Task<IActionResult> AddImages([FromForm] NewsImageUploadCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("images/reorder")]
        [Authorize]
        public async Task<IActionResult> ReorderImages(NewsImageReorderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("images")]
        [Authorize]
        public async Task<IActionResult> DeleteImages([FromQuery] DeleteNewsImageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
