using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Data.Persistence;
using Webshop.Domain.Common;
using Webshop.Order.Domain.AggregateRoots;
using Webshop.Order.Application.Contracts.Persistence;
using Dapper;
using Webshop.Domain.ValueObjects;

namespace Webshop.Order.Persistence
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(TableNames.Order.ORDERTABLE, dataContext)
        {
        }

        public async Task<Result> CreateAsync(Domain.AggregateRoots.Order entity)
        {
            using var connection = dataContext.CreateConnection();
            var command = $"insert into {TableName} (CustomerId, DateOfIssue, DueDate, Discount) values (@customerId, @dateOfIssue, @dueDate, @discount)";
            var createdOrderId = connection.QuerySingle<int>(command, new { name = entity.Customer, dateOfIssue = DateTime.UtcNow, dueDate = DateTime.UtcNow.AddDays(7), discount = entity.Discount});
            if (createdOrderId == 0)
            {
                return Result.Fail(new Error("DatabaseAccess", "Could not create an order row."));

            }
            foreach (var product in entity.OrderedProducts)
            {
                var manyToManyCommand = $"insert into {TableNames.Order.ORDERPRODUCTTABLE} (OrderId, ProductId, Quantity) values (@orderId, @productId, @quantity)";
                var rows = await connection.ExecuteAsync(manyToManyCommand, new { orderId = createdOrderId, product = product.Key.Id, quantity = product.Value });
                if (rows == 0)
                {
                    return Result.Fail(new Error("DatabaseAccess", "Could not create a many-to-many entry."));
                }
            }

            return Result.Ok();
        }

        public async Task<Domain.AggregateRoots.Order> GetAsync(int id)
        {
            using var connection = dataContext.CreateConnection();
            var query = $"select * from {TableName} where id = @id";
            return await connection.QuerySingleAsync<Domain.AggregateRoots.Order>(query, new { id });

        }

        public async Task<IEnumerable<Domain.AggregateRoots.Order>> GetAllAsync()
        {
            using var connection = dataContext.CreateConnection();
            var query = $"select * from {TableName}";
            return await connection.QueryAsync<Domain.AggregateRoots.Order>(query);

        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = dataContext.CreateConnection();
            var command = $"delete from {TableName} where Id = @id";

            return await connection.ExecuteAsync(command, new { id });
        }
    }
}
