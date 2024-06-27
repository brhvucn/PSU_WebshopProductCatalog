using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Review.Application.Features.GetAggregatedByProduct
{
    /// <summary>
    /// Example on how DTO can be used to transfer data between layers an to the API client
    /// </summary>
    public class ProductReviewDTO
    {
        public int ProductId { get; set; }
        public int AmountOfReviews { get; set; }
        public double AverageRating { get; set; }
    }
}
