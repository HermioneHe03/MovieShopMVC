using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class MovieReviewResponseModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public string Name { get; set; } 

        public static MovieReviewResponseModel FromEntity(Review review)
        {
            return new MovieReviewResponseModel
            {
                UserId = review.UserId,
                MovieId = review.MovieId,
                ReviewText = review.ReviewText,
                Rating = review.Rating

            };
        }
    }
}
