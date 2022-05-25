using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Contracts.Services;
using Application_Core.Entities;
using Application_Core.Models;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            var newPurchase =
                await _userRepository.AddNewPurchase(purchaseRequest.UserId, purchaseRequest.MovieId, purchaseRequest.TotalPrice);

            return newPurchase != null;
        }

        public async Task DeletePurchase(PurchaseRequestModel purchaseRequestModel)
        {
            await _userRepository.DeletePurchase(purchaseRequestModel.UserId, purchaseRequestModel.MovieId);
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchase = await _userRepository.GetPurchaseByUserAndMovie(purchaseRequest.UserId, purchaseRequest.MovieId);
            return purchase != null;
        }

        public async Task<PurchaseResponseModel> GetAllPurchasesForUser(int id)
        {

            var user = await _userRepository.GetById(id);
            var movies = await _userRepository.GetAllMoviesPurchasedByUser(id);
            var purchaseModel = PurchaseResponseModel.FromEntity(user, movies);

            return purchaseModel;

        }

        public async Task<PurchaseDetailsResponseModel> GetPurchasesDetails(int userId, int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            int userId = favoriteRequest.UserId;
            int movieId = favoriteRequest.MovieId;
            var favorite = await _userRepository.AddNewFavorite(userId, movieId);
        }

        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            int userId = favoriteRequest.UserId;
            int movieId = favoriteRequest.MovieId;
            var removedFavorite = await _userRepository.RemoveFavorite(userId, movieId);
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            Favorite favorite = await _userRepository.GetFavoriteByUserAndMovie(id, movieId);
            return favorite != null;
        }

        // not tested yet
        public async Task<FavoriteResponseModel> GetAllFavoritesForUser(int id)
        {
            var user = await _userRepository.GetById(id);
            var movies = await _userRepository.GetAllMoviesFavoritedByUser(id);
            var favoriteModel = FavoriteResponseModel.FromEntity(user, movies);

            return favoriteModel;
        }

        public async Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var newReview = await _userRepository.AddNewReview(
                reviewRequest.UserId,
                reviewRequest.MovieId,
                reviewRequest.Rating,
                reviewRequest.ReviewText
            );

        }

        public async Task PutMovieReview(ReviewRequestModel reviewRequest)
        {
            await _userRepository.PutMovieReview(reviewRequest.UserId, reviewRequest.MovieId, reviewRequest.Rating, reviewRequest.ReviewText);
        }

        public async Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var updatedReview = await _userRepository.UpdateReview(
                reviewRequest.UserId,
                reviewRequest.MovieId,
                reviewRequest.Rating,
                reviewRequest.ReviewText
            );
        }

        public async Task DeleteMovieReview(int userId, int movieId)
        {
            var deletedReview = await _userRepository.DeleteReviewByUserAndMovie(userId, movieId);
        }

        public async Task<UserReviewResponseModel> GetAllReviewsByUser(int id)
        {
            var user = await _userRepository.GetById(id);
            var reviews = await _userRepository.GetAllReviewsOfUser(id);
            var reviewModel = UserReviewResponseModel.FromEntity(user, reviews);

            return reviewModel;
        }
    }
}
