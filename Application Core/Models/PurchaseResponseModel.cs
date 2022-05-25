using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class PurchaseResponseModel
    {
        public int UserId { get; set; }

        public int TotalMoviesPurchased;
        public List<MovieCardModel> MovieCards { get; set; }

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.UserId + "\n");
            sb.Append("Total movie purchased: " + this.TotalMoviesPurchased + "\n");
            foreach (var card in MovieCards)
            {
                sb.Append(card.ToString() + "\n");
            }

            return sb.ToString();
        }

        public static PurchaseResponseModel FromEntity(User user, List<Movie> movies)
        {
            var cards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                cards.Add(MovieCardModel.FromEntity(movie));
            }

            return new PurchaseResponseModel { UserId = user.Id, MovieCards = cards, TotalMoviesPurchased = cards.Count };
        }

    }
}
