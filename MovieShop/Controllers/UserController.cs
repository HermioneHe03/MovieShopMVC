using System.Security.Claims;
using Application_Core.Contracts.Services;
using Application_Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private int GetUserId()
        {
            return Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            // first whether user is loged in
            // if the user is not loged in, 
            // redirect to login Page
            // cookies, authrication cookies that can be used across http request and check whether user is log in or not
            // userId, go to purchase table and get all the movies purchased
            // display as movie cards, use movie car partial view
            var userId = GetUserId();
            var model = await _userService.GetAllPurchasesForUser(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy()
        {

            int movieId = Convert.ToInt32(Request.Form["movieId"]);
            int userId = GetUserId();
            decimal totalPrice = Convert.ToDecimal(Request.Form["totalPrice"]);

            var purchaseModel = new PurchaseRequestModel
            {
                MovieId = movieId,
                UserId = userId,
                PurchaseDateTime = DateTime.Now,
                TotalPrice = totalPrice
            };

            var purchaseSucceed = await _userService.PurchaseMovie(purchaseModel, userId);

            return RedirectToAction("Purchases");
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = GetUserId();
            var model = await _userService.GetAllFavoritesForUser(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite()
        {
            int movieId = Convert.ToInt32(Request.Form["movieId"]);
            int userId = GetUserId();
            var favoriteModel = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };
            await _userService.AddFavorite(favoriteModel);

            return RedirectToAction("Favorites");
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var userId = GetUserId();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Review()
        {
            int movieId = Convert.ToInt32(Request.Form["movieId"]);
            int userId = GetUserId();
            decimal rating = Convert.ToDecimal(Request.Form["rating"]);
            var reviewText = Request.Form["reviewText"];

            var reviewModel = new ReviewRequestModel
            {
                MovieId = movieId,
                UserId = userId,
                Rating = rating,
                ReviewText = reviewText
            };

            await _userService.AddMovieReview(reviewModel);
            return RedirectToAction("Purchases");
        }


    }
}
