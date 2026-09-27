using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.IO;

namespace Webshop.Help.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string connectionString; //the server connectionstring without database
        private string mainconnectionString;
        private string server = "localhost";
        private List<string> Errors = new List<string>();
        private readonly string _contentRoot;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config, IWebHostEnvironment env)
        {
            _logger = logger;
            this.connectionString = config.GetConnectionString("DefaultConnection");
            this.mainconnectionString = this.connectionString;
            _contentRoot = env.ContentRootPath;
            string newServer = Environment.GetEnvironmentVariable("SERVER");
            if (!string.IsNullOrEmpty(newServer))
            {
                this.server = newServer;
            }
            this.mainconnectionString = this.mainconnectionString.Replace("{server}", this.server);
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            //create the database
            this.connectionString = this.mainconnectionString + ";database=master";
            CreateDatabase();
            CreateReviewDatabase();//creating psureviews database
            this.connectionString = this.mainconnectionString + ";database=psuwebshop"; //make sure they are created in the right database
            CreateCategoryTable();
            CreateCustomerTable();
            CreateProductTable();
            CreateProductCategoryTable();
            this.connectionString = this.mainconnectionString + ";database=PSUReviews"; //make sure they are created in the right database
            CreateReviewsTable();
            TempData["errors"] = Errors;
            return Redirect("/?seed=1");
        }

        public IActionResult OnPostReset()
        {
            Errors.Clear();
            this.connectionString = this.mainconnectionString + ";database=master";
            
            // Kill all active connections to psuwebshop database
            string killConnectionsSql = @"
                DECLARE @kill varchar(8000) = '';
                SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id = DB_ID('psuwebshop');
                EXEC(@kill);";
            ExecuteSQL(killConnectionsSql, this.connectionString);
            
            // Kill all active connections to PSUReviews database
            string killConnectionsReviewsSql = @"
                DECLARE @kill varchar(8000) = '';
                SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id = DB_ID('PSUReviews');
                EXEC(@kill);";
            ExecuteSQL(killConnectionsReviewsSql, this.connectionString);
            
            // drop databases if exist
            ExecuteSQL("DROP DATABASE IF EXISTS psuwebshop", this.connectionString);
            ExecuteSQL("DROP DATABASE IF EXISTS PSUReviews", this.connectionString);
            
            // recreate and seed basic schema
            CreateDatabase();
            CreateReviewDatabase();
            this.connectionString = this.mainconnectionString + ";database=psuwebshop";
            CreateCategoryTable();
            CreateCustomerTable();
            CreateProductTable();
            CreateProductCategoryTable();
            this.connectionString = this.mainconnectionString + ";database=PSUReviews";
            CreateReviewsTable();
            TempData["errors"] = Errors;
            return Redirect("/?reset=1");
        }

        public IActionResult OnPostSeed()
        {
            Errors.Clear();
            // ensure target database exists
            this.connectionString = this.mainconnectionString + ";database=psuwebshop";
            // create table if not exists
            CreateCustomerTable();
            // run Customers.sql then Seed Demo Customers.sql
            try
            {
                string customersSqlPath = Path.Combine(_contentRoot, "Customers.sql");
                if (System.IO.File.Exists(customersSqlPath))
                {
                    string sql = System.IO.File.ReadAllText(customersSqlPath);
                    ExecuteSQL(sql, this.connectionString);
                }

                string seedPath = Path.Combine(_contentRoot, "Seed Demo Customers.sql");
                if (System.IO.File.Exists(seedPath))
                {
                    string seedSql = System.IO.File.ReadAllText(seedPath);
                    ExecuteSQL(seedSql, this.connectionString);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex.Message);
            }

            TempData["errors"] = Errors;
            return Redirect("/?seed=1");
        }

        private void CreateDatabase()
        {            
            ExecuteSQL("IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'psuwebshop') CREATE DATABASE psuwebshop", this.connectionString);           
        }

        private void CreateReviewDatabase()
        {
            ExecuteSQL("IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PSUReviews') CREATE DATABASE PSUReviews", this.connectionString);
        }

        private void CreateReviewsTable()
        {
            string sql = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Reviews](
	            [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	            [ProductId] [int] NOT NULL,
	            [UserId] [int] NOT NULL,
	            [Comment] [nvarchar](max) NOT NULL,
	            [Rating] [int] NOT NULL,
	            [Created] [datetime] NOT NULL DEFAULT (getdate()))
            END";
            
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateCategoryTable()
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U')) " +
            "BEGIN " +
            "CREATE TABLE Category(" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[ParentId] [int] NOT NULL," +
            "[Description] [ntext] NOT NULL," +
            "CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            ") " +
            "END";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateCustomerTable()
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U')) " +
            "BEGIN " +
            "CREATE TABLE Customer(" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[Address] [nvarchar](200) NOT NULL," +
            "[Address2] [nvarchar](200) NULL," +
            "[City] [nvarchar](200) NOT NULL," +
            "[Region] [nvarchar](200) NOT NULL," +
            "[PostalCode] [nvarchar](50) NOT NULL," +
            "[Country] [nvarchar](150) NOT NULL," +
            "[Email] [nvarchar](100) NOT NULL," +
            "CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            ") " +
            "END";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateProductTable()
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U')) " +
            "BEGIN " +
            "CREATE TABLE Product(" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[SKU] [nvarchar](50) NOT NULL," +
            "[Price] [int] NOT NULL," +
            "[Currency] [nvarchar](3) NOT NULL," +
            "[Description] [ntext] NULL," +
            "[AmountInStock] [int] NULL," +
            "[MinStock] [int] NULL," +
            "CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            ") " +
            "END";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateProductCategoryTable()
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductCategory]') AND type in (N'U')) " +
            "BEGIN " +
            "CREATE TABLE ProductCategory(" +
            "[ProductId] [int] NOT NULL," +
            "[CategoryId] [int] NOT NULL," +
            "CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED " +
            "(" +
            "[ProductId] ASC," +
            "[CategoryId] ASC" +
            ")" +
            ") " +
            "END";
            ExecuteSQL(sql, this.connectionString);
        }

        private void ExecuteSQL(string sql, string localConnectionString)
        {
            try
            {
                Console.WriteLine("Connection: " + localConnectionString);
                using (SqlConnection connection = new SqlConnection(localConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex)
            {
                Errors.Add(ex.Message);
            }
        }
    }
}