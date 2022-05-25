using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Entities;
using Application_Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<Movie>> GetTop30GrossingMovies()
        {
            // SQL Database
            // data access logic
            // ADO.NET (Microsoft) SQL Connection, SQLCommand
            // Dapper (ORM) -> StackOverflow
            // Entity Framework Core => LINQ
            // SELECT top 30 * FROM Movie order by Revenue
            // movies.orderbydescnding(m=> m.Revenue).Take(30)
            //they provide both sync and async methods
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<List<Movie>> GetTop30RatingMovies()
        {
            var ids = await _dbContext.Review.Include(r => r.Movie)
            .GroupBy(r => r.MovieId)
            .Select(g => new
            {
                MovieId = g.Key,
                AvgRate = g.Average(r => r.Rating)
            })
            .OrderByDescending(rg => rg.AvgRate)
            .Select(rg => rg.MovieId)
            .Take(30)
            .ToListAsync();

            // reference: 
            // https://stackoverflow.com/questions/15275269/sort-a-list-from-another-list-ids
            var movies = await _dbContext.Movies
                .Where(m => ids.Contains(m.Id))
                // .OrderBy(m => ids.IndexOf(m.Id)) // does not work
                .ToListAsync();

            return movies;
        }

        public override async Task<Movie> GetById(int id)
        {
            // we need to join Navigation properties
            // Include method in EF will enable us to join with related navigation properties
            var movie = await _dbContext.Movies.Include(m => m.MoviesOfGenre).ThenInclude(m => m.Genre)
                .Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .FirstOrDefaultAsync(m => m.Id == id);
            //FirstOrDefault safest one
            //First throws ex when 0 records
            //SingleOrDefault good for 0 or 1
            //Single throw ex when no records found or when more than 1 records found

            return movie;

        }



        public async Task<List<Cast>> GetCastsByMovie(int movieId)
        {
            var casts = await _dbContext.MovieCast
            .Include(mc => mc.Cast)
            .Where(mc => mc.MovieId == movieId)
            .Select(mc => mc.Cast)
            .OrderBy(c => c.Id)
            .Take(10)
            .ToListAsync();

            return casts;
        }

        public async Task<List<MovieCast>> GetMovieCastsByMovie(int movieId)
        {
            var movieCasts = await _dbContext.MovieCast
           .Where(mc => mc.MovieId == movieId)
           .OrderBy(mc => mc.CastId)
           .Take(10)
           .ToListAsync();

            return movieCasts;
        }

        public async Task<List<Genre>> GetGenresOfMovie(int movieId)
        {
            var genres = await _dbContext.MovieGenre
            .Include(mg => mg.Genre)
            .Where(mg => mg.MovieId == movieId)
            .Select(mg => mg.Genre)
            .Take(10)
            .ToListAsync();

            return genres;
        }

        public async Task<List<Movie>> GetMoviesOfGenre(int genreId)
        {
            var movies = await _dbContext.MovieGenre
            .Include(mg => mg.Movie)
            .Where(mg => mg.GenreId == genreId)
            .Select(mg => mg.Movie)
            .Take(10)
            .ToListAsync();

            return movies;
        }

        public async Task<List<Review>> GetReviewsOfMovie(int movieId)
        {
            var reviews = await _dbContext.Review
            .Where(r => r.MovieId == movieId)
            .Take(10)
            .ToListAsync();

            return reviews;
        }

        public async Task<List<Trailer>> GetTrailersOfMovie(int movieId)
        {
            var trailers = await _dbContext.Trailer
            .Where(t => t.MovieId == movieId)
            .Take(10)
            .ToListAsync();

            return trailers;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByTitle(int pageSize = 30, int pageNumber = 1, string title = "")
        {
            var movies = await _dbContext.Movies
           .Where(m => m.Title.Contains(title))
           .OrderBy(m => m.Title)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            // total movies for that condition
            // SELECT COUNT(*) From Movies WHERE Title LIKE ..
            var totalMoviesCount = await _dbContext.Movies
                .Where(m => m.Title.Contains(title))
                .CountAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCount);

            return pagedMovies;
        }

        public async Task<PagedResultSet<Movie>> GetTopPurchasedMovies(int pageSize = 30, int pageNumber = 1)
        {
            var movieIds = await _dbContext.Purchase
           .GroupBy(p => p.MovieId)
           .Select(p => new
           {
               MovieId = p.Key,
               PurchaseCount = p.Count()
           })
           .OrderByDescending(pg => pg.PurchaseCount)
           .Select(pg => pg.MovieId)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            var movies = await _dbContext.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync();

            var totalMoviesCount = await _dbContext.Movies.CountAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCount);

            return pagedMovies;
        }

        public async Task<PagedResultSet<Movie>> MoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            //get total movies count for that genre
            var totalMoviesCountByGenre = await _dbContext.MovieGenre.Where(m => m.GenreId == genreId).CountAsync();
            // get the actual movies from MovieGenre and Movie Table
            if (totalMoviesCountByGenre == 0)
            {
                throw new Exception("No Movies Found for that genre");
            }

            var movies = await _dbContext.MovieGenre.Where(g => g.GenreId == genreId).Include(m => m.Movie)
                .OrderBy(m => m.MovieId)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies.ToList(), pageNumber, pageSize, totalMoviesCountByGenre);
            return pagedMovies;
        }
    }
}
