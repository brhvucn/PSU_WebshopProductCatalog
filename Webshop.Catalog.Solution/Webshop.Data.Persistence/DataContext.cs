using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Data.Persistence
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogger<DataContext> _logger;
        public DataContext(IConfiguration configuration, ILogger<DataContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
            //first check the environment variable: connectionstring
            //prefer the environment variable over the appsettings.json
            string envConnectionString = Environment.GetEnvironmentVariable("connectionstring");
            if (!string.IsNullOrEmpty(envConnectionString))
            {
                _connectionString = envConnectionString;
                this._logger.LogInformation($"Using connectionstring: \"{_connectionString}\" - from environment variable");
            }
            else
            {
                _connectionString = _configuration.GetConnectionString("DefaultConnection");
                this._logger.LogInformation($"Using connectionstring: \"{_connectionString}\" - from settings file");
            }            
        }
        public IDbConnection CreateConnection()
            => new System.Data.SqlClient.SqlConnection(_connectionString);
    }
}
