using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VUTTR.Backend.Data;
using VUTTR.Backend.Models;

namespace VUTTR.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ToolsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromServices] IToolsRespository toolsRepo,
            [FromQuery] string tag = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tag))
                return Ok(await toolsRepo.GetAll());

            return Ok(await toolsRepo.GetAllByTag(tag));
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(
            [FromServices] IToolsRespository toolsRepo,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var item = await toolsRepo.GetById(id, cancellationToken);
            if (item != null)
                return Ok(item);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] IToolsRespository toolsRepo,
            [FromBody] Tool data,
            CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                var tool = await toolsRepo.Create(data, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = tool.Id }, tool);
            }
            return BadRequest();
        }
    }
}
