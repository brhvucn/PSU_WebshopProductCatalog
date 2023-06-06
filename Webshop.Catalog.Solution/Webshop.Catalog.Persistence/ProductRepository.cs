using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Data.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Persistence
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(DataContext context) : base(TableNames.Catalog.PRODUCTTABLE, context) { }

        public async Task<Result> AddProductToCategory(int productId, int categoryId)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableNames.Catalog.PRODUCTCATEGORYTABLE} (productId, categoryId) values (@pid, @cid)";
                await connection.ExecuteAsync(command, new {pid = productId, cid = categoryId});
                return Result.Ok();
            }
        }

        public async Task CreateAsync(Product entity)
        {
            using(var connection  = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (Name, SKU, Price, Currency, Description, AmountInStock, MinStock) values (@name, @sku, @price, @currency, @description, @stock, @minstock)";
                await connection.ExecuteAsync(command, new
                {
                    name = entity.Name,
                    sku = entity.SKU,
                    price = entity.Price,
                    currency = entity.Currency,
                    description = entity.Description,
                    stock = entity.AmountInStock,
                    minstock = entity.MinStock
                });

            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableName} where id =  @id";
                await connection.ExecuteAsync(command, new { id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName}";
                return await connection.QueryAsync<Product>(query);
            }
        }

        public async Task<IEnumerable<Product>> GetAllFromCategory(int categoryId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} a join {TableNames.Catalog.PRODUCTCATEGORYTABLE} b on a.Id = b.ProductId where b.CategoryId = @categoryid";
                return await connection.QueryAsync<Product>(query, new {categoryid = categoryId});
            }
        }

        public async Task<Product> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Product>(query, new {id = id});
            }
        }

        public async Task<Result> RemoveProductFromCategory(int productId, int categoryId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableNames.Catalog.PRODUCTCATEGORYTABLE} where productId = @pid and categoryId = @cid";
                await connection.ExecuteAsync(command, new { pid = productId, cid = categoryId });
                return Result.Ok();
            }
        }

        public async Task<IEnumerable<Product>> GetProductsForOrderAsync(int orderId)
        {
            using var connection = dataContext.CreateConnection();
            var query = $"select p.Name, p.Description, p.SKU, p.AmountInStock, p.Price, p.Currency, p.MinStock " +
                        $"from {TableName} as p " +
                        $"inner join {TableNames.Order.ORDERPRODUCTTABLE} op " +
                        $"on p.Id = op.ProductId " +
                        $"inner join {TableNames.Order.ORDERTABLE} o " +
                        $"on op.OrderId = o.Id " +
                        $"where o.Id = @orderId";
            return await connection.QueryAsync<Product>(query, new { orderId });
        }

        public async Task UpdateAsync(Product entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set name = @name, description =  @desc, currency = @curr, price = @price, AmountInStock = @amount, MinStock = @min where id = @id";
                await connection.ExecuteAsync(command, new { 
                    name = entity.Name, 
                    desc = entity.Description, 
                    curr = entity.Currency, 
                    price = entity.Price, 
                    amount = entity.AmountInStock, 
                    min = entity.MinStock, 
                    id = entity.Id 
                });

            }
        }
    }
}
