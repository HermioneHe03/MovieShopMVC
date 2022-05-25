using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Models;

namespace Application_Core.Contracts.Services
{
    public interface IMovieService
    {
       //home/index action method
        Task<List<MovieCardModel>> GetTop30GrossingMovies();
        Task<List<MovieCardModel>> GetTop30RatingMovies();

        Task<MovieDetailsModel> GetMovieDetails(int id);
        Task<List<MovieCardModel>> GetMoviesOfGenre(int genreId);
        Task<List<MovieReviewResponseModel>> GetReviewsOfMovie(int movieId);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int pageSize, int page, string title);
        Task<PagedResultSet<MovieCardModel>> GetTopPurchasedMoviesByPagination(int pageSize, int pageNumber);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1);
    }
}
