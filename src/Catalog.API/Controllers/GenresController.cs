using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [JsonException]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _genreService.GetGenresAsync();

            var total = result.Count();

            var itemsOnPage = result
                .OrderBy(a => a.GenreDescription)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemsResponseModel<GenreResponse>(
                pageIndex, pageSize, total, itemsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _genreService.GetGenreAsync(new GetGenreRequest { Id = id });

            return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        public async Task<IActionResult> GetItemsById(Guid id)
        {
            var result = await _genreService.GetItemByGenreIdAsync(new GetGenreRequest { Id = id });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddGenreRequest request)
        {
            var result = await _genreService.AddGenreAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.GenreId }, null);
        }
    }
}
