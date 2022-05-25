using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class UserReviewResponseModel
    {
        public int UserId;
        public List<MovieReviewResponseModel> Reviews;

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.UserId + "\n");

            foreach (var review in this.Reviews)
            {
                sb.Append(review.ToString() + "\n");
            }

            return sb.ToString();
        }

        public static UserReviewResponseModel FromEntity(User user, List<Review> reviews)
        {
            var reviewModels = new List<MovieReviewResponseModel>();
            foreach (var review in reviews)
            {
                reviewModels.Add(MovieReviewResponseModel.FromEntity(review));
            }

            return new UserReviewResponseModel { UserId = user.Id, Reviews = reviewModels };
        }
    }
}
