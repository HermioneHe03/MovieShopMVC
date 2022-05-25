using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Contracts.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<List<Purchase>> GetAllPurchasesOfUser(int id);
        Task<Purchase> GetPurchaseByUserAndMovie(int userId, int movieId);
        Task<List<Movie>> GetAllMoviesPurchasedByUser(int userId);
        Task<Purchase> AddNewPurchase(int userId, int movieId, decimal price);
        Task DeletePurchase(int userId, int movieId);
        Task<List<Favorite>> GetAllFavoritesOfUser(int id);
        Task<Favorite> GetFavoriteByUserAndMovie(int userId, int movieId);
        Task<List<Movie>> GetAllMoviesFavoritedByUser(int userId);
        Task<Favorite> AddNewFavorite(int userId, int movieId);
        Task<Favorite> RemoveFavorite(int userId, int movieId);
        Task<List<Review>> GetAllReviewsOfUser(int id);
        Task<Review> GetReviewByUserAndMovie(int userId, int movieId);
        Task<Review> AddNewReview(int userId, int movieId, decimal rating, string text);
        Task<Review> UpdateReview(int userId, int movieId, decimal rating, string text);
        Task PutMovieReview(int userId, int movieId, decimal rating, string text);
        Task<Review> DeleteReviewByUserAndMovie(int userId, int movieId);
    }
}
