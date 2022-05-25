using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class FavoriteResponseModel
    {
        public int UserId { get; set; }
        public List<MovieCardModel> MovieCards { get; set; }

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.UserId + "\n");
            foreach (var movie in this.MovieCards)
            {
                sb.Append(movie.ToString() + "\n");
            }

            return sb.ToString();
        }

        public static FavoriteResponseModel FromEntity(User user, List<Movie> movies)
        {
            var cards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                cards.Add(MovieCardModel.FromEntity(movie));
            }

            return new FavoriteResponseModel { UserId = user.Id, MovieCards = cards };
        }
    }
}
