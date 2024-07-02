using Dapper;
using Microsoft.Extensions.Logging;
using Webshop.Data.Persistence;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Application.Features.GetAggregatedByProduct;

namespace Webshop.Review.Persistence
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(DataContext dataContext, ILogger<ReviewRepository> logger) : base(TableNames.Review.REVIEWTABLE, dataContext)
        {
            this._logger = logger;
        }

        public async Task CreateAsync(Domain.Review entity)
        {
            try
            {
                string sql = $"insert into {this.TableName} (productid, userid, comment, rating, created) values (@pid, @uid, @comment, @rating, @created)";
                using (var connection = dataContext.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, new { pid = entity.ProductId, uid = entity.UserId, comment = entity.Comment, rating = entity.Rating, created = entity.Created });
                }                
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw new Exception("Error in inserting review into database", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                string sql = $"delete from {this.TableName} where id = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, new { id = id });
                }                
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
            }
        }

        public async Task<Result<ProductReviewDTO>> GetAggregatedByProduct(int productId)
        {
            try
            {
                string sql = $"SELECT " +
                    $" r.ProductId AS ProductId," +
                    $" COUNT(r.Id) AS [AmountOfReviews]," +
                    $" AVG(CONVERT(float, r.Rating)) AS [AverageRating] " +
                    $" FROM" +
                    $" [PSUMicroservices_ReviewService].[dbo].[Reviews] r" +
                    $" WHERE" +
                    $" r.ProductId = @id  " +
                    $" GROUP BY" +
                    $" r.ProductId;";
                using (var connection = dataContext.CreateConnection())
                {
                    var result = await connection.QuerySingleOrDefaultAsync<ProductReviewDTO>(sql, new { id = productId });
                    if (result != null)
                    {
                        return Result.Ok<ProductReviewDTO>(result);
                    }
                    else
                    {
                        return Result.Fail<ProductReviewDTO>(Errors.General.NotFound<int>(productId));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<ProductReviewDTO>(Errors.General.FromException(ex));
            }
        }

        public Task<IEnumerable<Domain.Review>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Review> GetById(int id)
        {
            try
            {
                string sql = $"select * from {this.TableName} where id = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<Domain.Review>(sql, new { id = id });
                    if (result != null)
                    {
                        return Result.Ok<Domain.Review>(result);
                    }
                    else
                    {
                        return Result.Fail<Domain.Review>(Errors.General.NotFound<int>(id));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message); 
                return Result.Fail<Domain.Review>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<List<Domain.Review>>> GetByProduct(int productId)
        {
            try
            {
                string sql = $"select * from {this.TableName} where productid = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<Domain.Review>(sql, new { id = productId });
                    if (result != null)
                    {
                        return Result.Ok<List<Domain.Review>>(result.ToList());
                    }
                    else
                    {
                        return Result.Fail<List<Domain.Review>>(Errors.General.NotFound<int>(productId));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<Domain.Review>>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<List<Domain.Review>>> GetByUser(int userId)
        {
            try
            {
                string sql = $"select * from {this.TableName} where userid = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<Domain.Review>(sql, new { id = userId });
                    if (result != null)
                    {
                        return Result.Ok<List<Domain.Review>>(result.ToList());
                    }
                    else
                    {
                        return Result.Fail<List<Domain.Review>>(Errors.General.NotFound<int>(userId));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<Domain.Review>>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid)
        {
            //make sure the result from the database (tablenames) match the Properties in the DTO, that way we can use Dapper to map the result to the DTO - directly
            string sql = $"select ProductId, count(id) as AmountOfReviews, SUM(Rating) / CAST(COUNT(Id) AS FLOAT) as AverageRating from [PSUMicroservices_ReviewService].[dbo].[Reviews]  where productid = @id group by productid";
            try
            {
                using (var connection = dataContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ProductReviewDTO>(sql, new { id = productid });
                    if (result != null)
                    {
                        return Result.Ok<ProductReviewDTO>(result);
                    }
                    else
                    {
                        return Result.Fail<ProductReviewDTO>(Errors.General.NotFound<int>(productid));
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Result<ProductReviewDTO>>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<List<Domain.Review>>> GetProductReviews(int productid)
        {
            try
            {
                string sql = $"select * from {this.TableName} where productid = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    var resultIEnum = await connection.QueryAsync<Domain.Review>(sql, new { id = productid });
                    var result = resultIEnum.ToList();
                    if (result != null)
                    {
                        return Result.Ok<List<Domain.Review>>(result);
                    }
                    else
                    {
                        return Result.Fail<Result<List<Domain.Review>>>(Errors.General.NotFound<int>(productid));
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Result<List<Domain.Review>>>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<List<Domain.Review>>> GetUserReviews(int userid)
        {
            try
            {
                string sql = $"select * from {this.TableName} where userid = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    var resultIEnum = await connection.QueryAsync<Domain.Review>(sql, new { id = userid });
                    var result = resultIEnum.ToList();
                    if (result != null)
                    {
                        return Result.Ok<List<Domain.Review>>(result);
                    }
                    else
                    {
                        return Result.Fail<Result<List<Domain.Review>>>(Errors.General.NotFound<int>(userid));
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Result<List<Domain.Review>>>(Errors.General.FromException(ex));
            }
        }

        public async Task UpdateAsync(Domain.Review entity)
        {
            try
            {
                string sql = $"update {this.TableName} set comment =  @comment, rating = @rating where id = @id";
                using (var connection = dataContext.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, new { id = entity.Id, comment = entity.Comment, rating = entity.Rating });
                }                
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
            }
        }
    }
}
