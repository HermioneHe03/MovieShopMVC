using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Contracts.Services;
using Infrastructure.Repositories;
using Application_Core.Models;

namespace Infrastructure.Services
{
    public class MovieService: IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<MovieDetailsModel> GetMovieDetails(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);
            var movieDetails = new MovieDetailsModel
            {
                Id = movie.Id,
                Budget = movie.Budget,
                Overview = movie.Overview,
                Price = movie.Price,
                PosterUrl = movie.PosterUrl,
                Revenue = movie.Revenue,
                ReleaseDate = movie.ReleaseDate,
                Tagline = movie.Tagline,
                Title = movie.Title,
                RunTime = movie.RunTime,
                BackdropUrl = movie.BackdropUrl,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
            };
            foreach (var trailer in movie.Trailers)
            {
                movieDetails.Trailers.Add(new TrailerModel { Id = trailer.Id, Name = trailer.Name, TrailerUrl = trailer.TrailerUrl });
            }
            foreach (var genre in movie.MoviesOfGenre)
            {
                movieDetails.Genres.Add(new GenreModel { Id = genre.GenreId, Name = genre.Genre.Name });

            }
            foreach (var cast in movie.MovieCasts)
            {
                movieDetails.Casts.Add(new CastModel { Id = cast.CastId, Name = cast.Cast.Name, Character = cast.Character, ProfilePath = cast.Cast.ProfilePath });
            }
            int counts = 0;
            decimal sum = 0;
            foreach (var review in movie.Reviews)
            {
                sum = sum + review.Rating;
                counts++;
            }
            movieDetails.Rating = Math.Round((decimal)sum / counts, 2);

            return movieDetails;

        }
        public async Task<List<MovieCardModel>> GetMoviesOfGenre(int id)
        {
            int genreId = id;

            var movies = await _movieRepository.GetMoviesOfGenre(genreId);
            List<MovieCardModel> movieModels = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieModels.Add(MovieCardModel.FromEntity(movie));
            }

            return movieModels;
        }

        public async Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int pageSize, int page,
        string title)
        {
            var pagedMovies = await _movieRepository.GetMoviesByTitle(pageSize, page, title);
            var pagedMovieCards = new List<MovieCardModel>();

            pagedMovieCards.AddRange(pagedMovies.Data.Select(
                m => new MovieCardModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    PosterUrl = m.PosterUrl
                }));

            return new PagedResultSet<MovieCardModel>(pagedMovieCards, page, pageSize, pagedMovies.Count);
        }

        public async Task<List<MovieCardModel>> GetTop30GrossingMovies()
        {
            //call the movierepository
            //get the entity class data and map them in to model class data
            // var movieRepo = new MovieRepository();
            //var movies = movieRepo.GetTop30GrossingMovies();
            var movies = await _movieRepository.GetTop30GrossingMovies();

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

        public async Task<List<MovieCardModel>> GetTop30RatingMovies()
        {
            var movies = await _movieRepository.GetTop30RatingMovies();
            var movieCards = new List<MovieCardModel>();

            foreach (var movie in movies)
            {
                movieCards.Add(MovieCardModel.FromEntity(movie));
            }

            return movieCards;
        }
        public async Task<PagedResultSet<MovieCardModel>> GetTopPurchasedMoviesByPagination(int pageSize, int page)
        {
            var pagedMovies = await _movieRepository.GetTopPurchasedMovies(pageSize, page);
            var pagedMovieCards = new List<MovieCardModel>();

            pagedMovieCards.AddRange(pagedMovies.Data.Select(
                    m => MovieCardModel.FromEntity(m)
                ));

            return new PagedResultSet<MovieCardModel>(pagedMovieCards, page, pageSize, pagedMovies.Count);
        }

        public async Task<List<MovieReviewResponseModel>> GetReviewsOfMovie(int movieId)
        {
            var reviews = await _movieRepository.GetReviewsOfMovie(movieId);
            var reviewModels = new List<MovieReviewResponseModel>();
            foreach (var review in reviews)
            {
                reviewModels.Add(MovieReviewResponseModel.FromEntity(review));
            }

            return reviewModels;
        }

        public async Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            var pagedMovies = await _movieRepository.GetMoviesOfGenre(genreId);
            var movieCards = new List<MovieCardModel>();

            movieCards.AddRange(pagedMovies.Select(x => new MovieCardModel
            {
                Id = x.Id,
                PosterUrl = x.PosterUrl,
                Title = x.Title,
            }));
            return new PagedResultSet<MovieCardModel>(movieCards, pageNumber, pageSize, pagedMovies.Count);
        }


    }
}
