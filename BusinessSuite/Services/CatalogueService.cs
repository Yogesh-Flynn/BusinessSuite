using BusinessSuite.Data;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BusinessSuite.Services
{
    public class CatalogueService : ICatalogueService
    {
        private readonly IDataService _dataService;

        private readonly ApplicationDbContext _dbContext;
        public CatalogueService(IDataService dataService,ApplicationDbContext dbContext) 
        {
            _dataService = dataService;
            _dbContext = dbContext;
        }


        public Task<bool> CreateColumnAsync(string ModuleName, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateModuleAsync(string ModuleName, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTableAsync(string TableName, string ModuleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
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

        public async Task<CataloguesViewModel> RetrieveAllTableAsync(int szDatabaseMasterId)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var result =await _dataService.RetrieveAllTableAsync(sqlConnectionString.ConnectionString);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
           

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
