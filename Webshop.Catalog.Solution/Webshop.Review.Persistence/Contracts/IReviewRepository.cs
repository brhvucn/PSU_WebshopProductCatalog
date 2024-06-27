using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;
using Webshop.Review.Application.Features.GetAggregatedByProduct;

namespace Webshop.Review.Persistence.Contracts
{
    /// <summary>
    /// This interface is located in the persistence layer and is used to define the contract for the ReviewRepository. This is because this is a 3 layered architecture where the persistence layer is the lowest layer and the API is the highest layer.
    /// Alternatively, if this was clean architecture, the interface would be in the application layer.
    /// </summary>
    public interface IReviewRepository : IBaseRepository<Domain.Review>
    {
        Task<Result<List<Domain.Review>>> GetUserReviews(int userid);
        Task<Result<List<Domain.Review>>> GetProductReviews(int productid);
        Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid);
    }

    /// <summary>
    /// This Interface is located here since this i
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T>
    {
        Task<Result<T>> GetByIdAsync(int id);
        Task<Result> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(int id);
    }
}
