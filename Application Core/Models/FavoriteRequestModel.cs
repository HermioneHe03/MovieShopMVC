using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class FavoriteRequestModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }

        public string ToString()
        {
            return this.UserId + " " + this.MovieId;
        }

        public static FavoriteRequestModel FromEntity(User user, Movie movie)
        {
            return new FavoriteRequestModel { UserId = user.Id, MovieId = movie.Id };
        }


    }
}
