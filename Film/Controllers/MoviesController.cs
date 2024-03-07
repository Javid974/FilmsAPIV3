
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DAO;
using Models.ViewModel;
using Services;
using Services.Exceptions;
using Services.Interfaces;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Film.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MoviesController> _logger;
        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _logger = logger;
            _movieService = movieService;
        }

        // GET: api/<ValuesController>
        [HttpGet("years/{years}")]
        public async Task<ActionResult<List<Movie>>> GetByViewingYearsDate(int years)
        {
            try
            {
                var movies = await _movieService.GetByViewingYearsDate(years);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(Guid id)
        {
            var movie = await _movieService.Get(id);
            return Ok(movie);
        }

        // GET api/<ValuesController>/5
        [HttpGet("download")]
        public async Task<ActionResult> Download()
        {
            return await _movieService.Download();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Movie movie)
        {
            var updatedMovie = await _movieService.Update(movie);
            return Ok(updatedMovie);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _movieService.Delete(id);
                return NoContent(); // Retourne un code de statut HTTP 204
            }
            catch (Exception ex)
            {
                // Gérer l'exception comme vous le souhaitez, par exemple, enregistrer l'erreur et retourner un code d'erreur HTTP
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie movie)
        {
            try
            {
                await _movieService.Create(movie);
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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var reponse = await _movieService.Upload(file);
                return Ok(new { message = reponse });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                await _movieService.Import(file);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

    }
}
