using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Entities;

namespace Application_Core.Models
{
    public class PurchaseRequestModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDateTime { get; set; }

        public static PurchaseRequestModel FromEntity(Purchase purchase)
        {
            var purchaseModel = new PurchaseRequestModel
            {
                UserId = purchase.UserId,
                MovieId = purchase.MovieId,
                TotalPrice = purchase.TotalPrice,
                PurchaseDateTime = purchase.PurchaseDateTime
            };

            return purchaseModel;
        }
    }
}
