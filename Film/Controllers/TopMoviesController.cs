
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using Services;
using Services.Exceptions;
using Services.Interfaces;

namespace Film.Controllers
{
    [Route("api/topmovies")]
    [ApiController]
    public class TopMoviesController : ControllerBase
    {
        // GET: TopMoviesController
        private readonly IMovieService _movieService;
        private readonly ITopMovieService _topMovieService;

        public TopMoviesController(IMovieService movieService, ITopMovieService topMovieService)
        {
            _movieService = movieService;
            _topMovieService = topMovieService;
        }

        [HttpGet("movies/years/{years}")]
        public async Task<ActionResult<List<Movie>>> GetByViewingYearsDate(int years)
        {
            var movies = await _movieService.GetByViewingYearsDate(years);
            return Ok(movies.Where(m => m.MightWatch.HasValue && !m.MightWatch.Value).OrderBy(m => m.Title).ToList());
        }

        [HttpGet("years/{years}")]
        public async Task<ActionResult<List<TopMovie>>> GetByYears(int years)
        {
            return Ok(await _topMovieService.GetByYears(years));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TopMovie>> Get(Guid id)
        {
            return Ok(await _topMovieService.Get(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] TopMovie topMovie)
        {
            return Ok(await _topMovieService.Update(topMovie));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TopMovie topMovie)
        {
            try
            {
                await _topMovieService.Create(topMovie);
                return Ok();
            }
            catch (ConflictException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _topMovieService.Delete(id);
                return NoContent(); // Retourne un code de statut HTTP 204
            }
            catch (Exception ex)
            {
                // Gérer l'exception comme vous le souhaitez, par exemple, enregistrer l'erreur et retourner un code d'erreur HTTP
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("download")]
        public async Task<ActionResult> Download()
        {
            return await _topMovieService.Download();
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                await _topMovieService.Import(file);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
