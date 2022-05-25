using System.Diagnostics;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Models;
using Infrastructure.Repositories;
using Application_Core.Contracts.Services;

namespace MovieShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;
        public HomeController(ILogger<HomeController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
            // code to be relied on abstractions rather than concrete types
        }

        //Action methods
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //newing up
            // we can have some higher level framework to create instances
            var movieCards = await  _movieService.GetTop30GrossingMovies();
            // passing the data from Controller action method to View
            return View(movieCards);
        }


        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TopRatedMovies()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}