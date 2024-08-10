using BusinessSuite.Models.ViewModels;
using System.Data;

namespace BusinessSuite.Interfaces
{
    public interface IDataService
    {

        public Task<bool> CreateWebsiteAsync(string WebsiteName);
        public Task<bool> RetrieveWebsiteAsync(string WebsiteName);
        public Task<bool> RetrieveAllWebsitesAsync();
        public Task<bool> UpdateWebsiteAsync(string WebsiteName);
        public Task<bool> DeleteWebsiteAsync(string WebsiteName);
        // module map
        public Task<bool> CreateModuleAsync(string szConnectionString, string WebsiteName);
        public Task<bool> RetrieveModuleAsync(string szConnectionString, string WebsiteName);
        public Task<bool> RetrieveAllModuleAsync(string WebsiteName);
        public Task<bool> UpdateModuleAsync(string szConnectionString, string WebsiteName);
        public Task<bool> DeleteModuleAsync(string szConnectionString, string WebsiteName);
        //table map
        public Task<bool> CreateTableAsync(string TableName, string szConnectionString);
        public Task<bool> RetrieveTableAsync(string TableName, string szConnectionString);
        public Task<CataloguesViewModel> RetrieveAllTableAsync(string szConnectionString);
        public Task<List<string>> RetrieveAllTableNameAsync(string szConnectionString);
        public Task<DataTable> RetrieveAllTableReferencesAsync(string szConnectionString, string sourceTable, string targetTable);
        public Task<bool> UpdateTableAsync(string TableName, string szConnectionString);
        public Task<bool> DeleteTableAsync(string TableName, string szConnectionString);
        //Column map
        public Task<bool> CreateColumnAsync(string szConnectionString, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> RetrieveColumnAsync(string szConnectionString, string TableName, string ColumnName);
        public Task<DataTable> RetrieveAllColumnAsync(string szConnectionString, string TableName);
        public Task<bool> UpdateColumnAsync(string szConnectionString, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> DeleteColumnAsync(string szConnectionString, string TableName, string ColumnName);
        // data map
        public Task<int> InsertDataAsync(string szConnectionString, string TableName,string Columns, string ColumnValues);
        public Task<int> UploadDataAsync(string szConnectionString, string TableName,DataTable dataTable);
        public Task<bool> RetrieveDataAsync(string szConnectionString, string TableName, string filter);
        public Task<DataTable> RetrieveAllDataAsync(string szConnectionString, string TableName);
        public Task<DataTable> RunCustomQueryAsync(string szConnectionString, string TableName);
        public Task<bool> UpdateDataAsync(string szConnectionString, string TableName, Dictionary<string, string> Data);
        public Task<bool> DeleteDataAsync(string szConnectionString, string TableName, string DataId);
    }
}
