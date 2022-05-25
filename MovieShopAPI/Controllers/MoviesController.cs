using Application_Core.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    // Atribute based routing
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // api/movies/top-grossing
        [Route("top-grossing")]
        [HttpGet]
        public async Task<IActionResult> TopGrossing()
        {
            var movies = await _movieService.GetTop30GrossingMovies();
            // return JSON data and always return proper HTTP status code

            if (movies == null || !movies.Any())
            {
                // 404
                return NotFound(new { errorMessage = "No Movies found" });
            }

            // ASP.NET Core API will automatcally serialize C# objects in to JSON Objects
            // System.Text.Json => 
            // If you are using .NET Core 2 or older or older .NET Framework then JSON serilazation arev done using a library called
            // Newtonsoft.json
            // 200 OK
            return Ok(movies);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { ErrorMessage = "No Movie FOund" });
            }
            return Ok(movie);
        }

    }
}

