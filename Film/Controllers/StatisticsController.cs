using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using Services;
using Services.Exceptions;
using Services.Interfaces;

namespace Film.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // GET: StatisticsController
        [HttpGet("years/{years}")]
        public async Task<ActionResult<Statistics>> Get(int years)
        {
            try
            {
                var stats = await _statisticsService.Get(years);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("directorMoviesCount")]
        public async Task<ActionResult<List<DirectorMoviesCount>>> Get()
        {
            try
            {
                return Ok(await _statisticsService.GetDirectorMoviesCount());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("moviesdirector/id/{id}")]
        public async Task<ActionResult<List<DirectorMoviesCount>>> GetMoviesByDirectorId(int id)
        {
            try
            {
                return Ok(await _statisticsService.GetMoviesByDirectorId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("director/id/{id}")]
        public async Task<ActionResult<Director?>> GetDirectorByDirectorId(int id)
        {
            try
            {
                return Ok(await _statisticsService.GetDirectorById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("download")]
        public async Task<ActionResult> Download()
        {
            return await _statisticsService.Download();
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                await _statisticsService.Import(file);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
