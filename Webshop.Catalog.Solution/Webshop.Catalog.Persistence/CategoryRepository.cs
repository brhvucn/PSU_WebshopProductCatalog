using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Data.Persistence;

namespace Webshop.Catalog.Persistence
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(DataContext context) : base(TableNames.Catalog.CATEGORYTABLE, context) { }
        public async Task CreateAsync(Catalog.Domain.AggregateRoots.Category entity)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (Name, ParentId, Description) values (@name, @parentid, @description)";
                await connection.ExecuteAsync(command, new { name = entity.Name, parentid = entity.ParentId, description = entity.Description });
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableName} where id = @id";
                await connection.ExecuteAsync(command, new { id = id });
            }
        }

        public async Task<bool> ExistsCategory(int parentId)
        {
            try
            {
                var category = await GetById(parentId);
                return category != null;
            }
            catch(Exception ex)
            {
                //probably returned an exception for an empty result. This means that it does not exist.
                return false;
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Category>> GetAll()
        {
            using(var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName}";
                return await connection.QueryAsync<Domain.AggregateRoots.Category>(query);
            }
        }

        public async Task<Domain.AggregateRoots.Category> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Category>(query, new {id = id});
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Category>> GetChildCategories(int parentCategory)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where parentid = @parentCategory";
                return await connection.QueryAsync<Domain.AggregateRoots.Category>(query, new {parentCategory = parentCategory});
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Category>> GetForProduct(int productId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} a join {TableNames.Catalog.PRODUCTCATEGORYTABLE} b on a.Id = b.CategoryId where b.ProductId = @productId";
                return await connection.QueryAsync<Domain.AggregateRoots.Category>(query, new { productId = productId });
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Category>> GetRootCategories()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where parentid = 0";
                return await connection.QueryAsync<Domain.AggregateRoots.Category>(query);
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Category entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set Name = @name, Description = @description where id = @id";
                await connection.ExecuteAsync(command, new {name = entity.Name, description = entity.Description, id = entity.Id});
            }
        }
    }
}
