using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Review.Domain;
using Webshop.Domain.Common;
using Webshop.Review.Application.Features.GetAggregatedByProduct;

namespace Webshop.Review.Application.Contracts.Persistence
{
    public interface IReviewRepository : IRepository<Domain.Review>
    {
        Task<Result<List<Domain.Review>>> GetUserReviews(int userid);
        Task<Result<List<Domain.Review>>> GetProductReviews(int productid);
        Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid);
        Task<Result<List<Domain.Review>>> GetByUser(int userId);
        Task<Result<List<Domain.Review>>> GetByProduct(int productId);
        Task<Result<ProductReviewDTO>> GetAggregatedByProduct(int productId);
    }
}
