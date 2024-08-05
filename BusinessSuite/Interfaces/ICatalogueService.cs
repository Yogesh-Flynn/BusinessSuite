using BusinessSuite.Models.ViewModels;
using System.Data;

namespace BusinessSuite.Interfaces
{
    public interface ICatalogueService
    { // Website map
        public Task<bool> CreateWebsiteAsync(string WebsiteName);
        public Task<bool> RetrieveWebsiteAsync(string WebsiteName);
        public Task<bool> RetrieveAllWebsitesAsync();
        public Task<bool> UpdateWebsiteAsync(string WebsiteName);
        public Task<bool> DeleteWebsiteAsync(string WebsiteName);
        // module map
        public Task<bool> CreateModuleAsync(int szDatabaseMasterId, string WebsiteName);
        public Task<bool> RetrieveModuleAsync(int szDatabaseMasterId, string WebsiteName);
        public Task<bool> RetrieveAllModuleAsync(string WebsiteName);
        public Task<bool> UpdateModuleAsync(int szDatabaseMasterId, string WebsiteName);
        public Task<bool> DeleteModuleAsync(int szDatabaseMasterId, string WebsiteName);
        //table map
        public Task<bool> CreateTableAsync(string TableName, int szDatabaseMasterId);
        public Task<bool> RetrieveTableAsync(string TableName,int szDatabaseMasterId);
        public Task<CataloguesViewModel> RetrieveAllTableAsync(int szDatabaseMasterId);
        public Task<DisplayTableViewModel> DisplayTableAsync(int szDatabaseMasterId, string szTableName);
        public Task<List<string>> RetrieveAllTableNameAsync(int szDatabaseMasterId);
        public Task<DataTable> RetrieveAllTableReferencesAsync(int szDatabaseMasterId,string sourceTable,string targetTable);
        public Task<bool> UpdateTableAsync(string TableName, int szDatabaseMasterId);
        public Task<bool> DeleteTableAsync(string TableName, int szDatabaseMasterId);
        //Column map
        public Task<bool> CreateColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> RetrieveColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName);
        public Task<DataTable> RetrieveAllColumnAsync(int szDatabaseMasterId, string TableName);
        public Task<bool> UpdateColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> DeleteColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName);
        // data map
        public Task<bool> InsertDataAsync(int szDatabaseMasterId, string TableName, Dictionary<string, string> Data);
        public Task<bool> RetrieveDataAsync(int szDatabaseMasterId, string TableName, string filter);
        public Task<DataTable> RetrieveAllDataAsync(int szDatabaseMasterId, string TableName);
        public Task<bool> UpdateDataAsync(int szDatabaseMasterId, string TableName, Dictionary<string, string> Data);
        public Task<bool> DeleteDataAsync(int szDatabaseMasterId, string TableName, string DataId);
    }
}
