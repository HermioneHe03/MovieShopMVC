using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Services;
using Infrastructure.Repositories;
using Application_Core.Models;
using Application_Core.Contracts.Repositories;

namespace Infrastructure.Services
{
    public class MovieTestService: IMovieService
    {
        private readonly IMovieRepository _movieRepo;
        public MovieTestService(IMovieRepository movieRepository)
        {
            _movieRepo = movieRepository;
        }

        public async Task<MovieDetailsModel> GetMovieDetails(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

      

        public Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int pageSize, int page, string title)
        {
            throw new NotImplementedException();
        }

        public Task<List<MovieCardModel>> GetMoviesOfGenre(int genreId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MovieReviewResponseModel>> GetReviewsOfMovie(int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MovieCardModel>> GetTop30GrossingMovies()
        {

            //call the movierepository
            //get the entity class data and map them in to model class data
            var movies = await _movieRepo.GetTop30GrossingMovies();
            var movieCards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title
                });
            }
            return movieCards;
        }

        public Task<List<MovieCardModel>> GetTop30RatingMovies()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultSet<MovieCardModel>> GetTopPurchasedMoviesByPagination(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }
    }
}
