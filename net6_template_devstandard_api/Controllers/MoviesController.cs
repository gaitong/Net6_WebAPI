using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net6_template_devstandard_api.Models;
using net6_template_devstandard_api.Services;

namespace net6_template_devstandard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService movieService;
        private readonly ILogger<MoviesController> logger;

        public MoviesController(IMovieService movieService
            , ILogger<MoviesController> logger)
        {
            this.movieService = movieService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                logger.LogDebug("Get movies");
                var res = await movieService.GetAll();
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                logger.LogDebug("Get by id {0}", id);
                var res = await movieService.GetById(id);
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Movie movie)
        {
            try
            {
                var res = await movieService.Add(movie);
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromBody] Movie movie)
        {
            try
            {
                var res = await movieService.Update(movie);
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await movieService.Delete(id);
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
