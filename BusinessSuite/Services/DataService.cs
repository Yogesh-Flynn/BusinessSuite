using BusinessSuite.Controllers;
using BusinessSuite.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessSuite.Services
{
    public class DataService : IDataService
    {
        private readonly MyJobService _jobService;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public DataService(MyJobService jobService, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _jobService = jobService;
            _logger = logger;
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("customDBConn"));
            
        }

        public Task<bool> CreateColumnAsync(string ModuleName, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateModuleAsync(string ModuleName, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateTableAsync(string TableName, string ModuleName)
        {
            try
            {
                string createTableQuery = $"CREATE TABLE {TableName} (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100), CreatedDate DATETIME)";
                await _connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the table.");
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return false;
        }

        public async Task<bool> CreateWebsiteAsync(string WebsiteName)
        {
            string connectionString = $"Server={_configuration.GetConnectionString("ServerName")};Integrated Security=true;";
            string createDatabaseQuery = $"CREATE DATABASE {WebsiteName}";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await _connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database created successfully.");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return false;
        }

        public Task<bool> DeleteColumnAsync(string ModuleName, string TableName, string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDataAsync(string ModuleName, string TableName, string DataId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteModuleAsync(string ModuleName, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTableAsync(string TableName, string ModuleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertDataAsync(string ModuleName, string TableName, Dictionary<string, string> Data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveAllColumnAsync(string ModuleName, string TableName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveAllDataAsync(string ModuleName, string TableName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveAllModuleAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveAllTableAsync(string ModuleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveAllWebsitesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveColumnAsync(string ModuleName, string TableName, string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveDataAsync(string ModuleName, string TableName, string filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveModuleAsync(string ModuleName, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveTableAsync(string TableName, string ModuleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateColumnAsync(string ModuleName, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDataAsync(string ModuleName, string TableName, Dictionary<string, string> Data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateModuleAsync(string ModuleName, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTableAsync(string TableName, string ModuleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }
    }
}
