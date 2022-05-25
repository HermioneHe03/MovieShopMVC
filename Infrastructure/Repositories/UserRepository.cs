using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public override async Task<User> GetById(int id)
        {
            var user = await _dbContext.User.Where(u => u.Id == id).SingleOrDefaultAsync();
            return user;
        }



        

        public async Task<List<Purchase>> GetAllPurchasesOfUser(int id)
        {
            var purchases = await _dbContext.Purchase
                .Where(p => p.UserId == id)
                .ToListAsync();

            return purchases;
        }

        public async Task<Purchase> GetPurchaseByUserAndMovie(int userId, int movieId)
        {
            var purchase = await _dbContext.Purchase
                .Where(p => p.UserId == userId && p.MovieId == movieId)
                .SingleOrDefaultAsync();

            return purchase;
        }

        public async Task<List<Movie>> GetAllMoviesPurchasedByUser(int userId)
        {
            var movies = await _dbContext.Purchase
                .Include(p => p.Movie)
                .Where(p => p.UserId == userId)
                .Select(p => p.Movie)
                .ToListAsync();
            return movies;
        }

        
        public async Task<Purchase> AddNewPurchase(int userId, int movieId, decimal price)
        {
            var purchase = await GetPurchaseByUserAndMovie(userId, movieId);

            if (purchase == null)
            {
                Purchase newPurchase = new Purchase
                {
                    UserId = userId,
                    MovieId = movieId,
                    TotalPrice = price,
                    PurchaseDateTime = DateTime.Now,
                    PurchaseNumber = Guid.NewGuid()
                };
                await _dbContext.Purchase.AddAsync(newPurchase);
                await _dbContext.SaveChangesAsync();

                return newPurchase;
            }

            return null;
        }


        public async Task DeletePurchase(int userId, int movieId)
        {
            var purchase = await GetPurchaseByUserAndMovie(userId, movieId);
            if (purchase != null)
            {
                _dbContext.Purchase.Remove(purchase);
                await _dbContext.SaveChangesAsync();
            }
        }

        
        public async Task<List<Favorite>> GetAllFavoritesOfUser(int id)
        {
            var favorites = await _dbContext.Favorite
                .Where(f => f.UserId == id)
                .ToListAsync();

            return favorites;
        }

        public async Task<Favorite> GetFavoriteByUserAndMovie(int userId, int movieId)
        {
            var purchase = await _dbContext.Favorite
                .Where(f => f.UserId == userId && f.MovieId == movieId)
                .SingleOrDefaultAsync();

            return purchase;
        }

        public async Task<List<Movie>> GetAllMoviesFavoritedByUser(int userId)
        {
            var movies = await _dbContext.Favorite
                .Include(f => f.Movie)
                .Where(f => f.UserId == userId)
                .Select(f => f.Movie)
                .ToListAsync();
            return movies;
        }

        public async Task<Favorite> AddNewFavorite(int userId, int movieId)
        {
            var favorite = await GetFavoriteByUserAndMovie(userId, movieId);
            if (favorite == null)
            {
                var newFavorite = new Favorite { UserId = userId, MovieId = movieId };
                await _dbContext.Favorite.AddAsync(newFavorite);
                await _dbContext.SaveChangesAsync();

                return newFavorite;
            }
            return null;
        }

        public async Task<Favorite> RemoveFavorite(int userId, int movieId)
        {
            var favorite = await GetFavoriteByUserAndMovie(userId, movieId);
            if (favorite != null)
            {
                _dbContext.Favorite.Remove(favorite);
                await _dbContext.SaveChangesAsync();
                return favorite;
            }

            return null;
        }

        
        public async Task<List<Review>> GetAllReviewsOfUser(int id)
        {
            var reviews = await _dbContext.Review
                .Where(r => r.UserId == id)
                .ToListAsync();

            return reviews;
        }

        public async Task<Review> GetReviewByUserAndMovie(int userId, int movieId)
        {
            var review = await _dbContext.Review
                .Where(r => r.UserId == userId && r.MovieId == movieId)
                .SingleOrDefaultAsync();

            return review;
        }

        public async Task<Review> AddNewReview(int userId, int movieId, decimal rating, string text)
        {
            var review = await GetReviewByUserAndMovie(userId, movieId);

            if (review == null)
            {
                var newReview = new Review { UserId = userId, MovieId = movieId, Rating = rating, ReviewText = text };
                await _dbContext.Review.AddAsync(newReview);
                await _dbContext.SaveChangesAsync();
                return review;
            }
            else
            {
                var newReview = await UpdateReview(userId, movieId, rating, text);
                return newReview;
            }
        }

        public async Task<Review> UpdateReview(int userId, int movieId, decimal rating, string text)
        {
            var review = await GetReviewByUserAndMovie(userId, movieId);

            if (review == null)
            {
                var newReview = await AddNewReview(userId, movieId, rating, text);
                return newReview;
            }
            else
            {

                var query = await _dbContext.Review
                    .Where(r => r.UserId == userId && r.MovieId == movieId)
                    .SingleOrDefaultAsync();

                query.Rating = rating;
                query.ReviewText = text;

                try
                {
                    await _dbContext.SaveChangesAsync();
                    return new Review { UserId = userId, MovieId = movieId, Rating = rating, ReviewText = text };
                }
                catch
                {
                    throw new DbUpdateException();
                }
            }
        }

        public async Task PutMovieReview(int userId, int movieId, decimal rating, string text)
        {
            var review = await GetReviewByUserAndMovie(userId, movieId);
            if (review == null)
            {
                await AddNewReview(userId, movieId, rating, text);
            }
            else
            {
                review.UserId = userId;
                review.MovieId = movieId;
                review.Rating = rating;
                review.ReviewText = text;
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<Review> DeleteReviewByUserAndMovie(int userId, int movieId)
        {
            var review = await GetReviewByUserAndMovie(userId, movieId);
            if (review == null)
            {
                return null;
            }
            else
            {
                _dbContext.Review.Remove(review);
                await _dbContext.SaveChangesAsync();
                return review;
            }
        }
    }
}
