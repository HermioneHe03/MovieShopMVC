

using Application_Core.Entities;

namespace Application_Core.Models
{
    public class MovieCardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ToString()
        {
            return this.Id + " " + this.Title + " " + this.PosterUrl;
        }

        public static MovieCardModel FromEntity(Movie movie)
        {
            MovieCardModel movieCard = new MovieCardModel
            {
                Id = movie.Id,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl
            };

            return movieCard;
        }
    }
}
