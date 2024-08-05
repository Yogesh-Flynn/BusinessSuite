using BusinessSuite.Models.ViewModels;

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
        public Task<bool> CreateModuleAsync(string ModuleName, string WebsiteName);
        public Task<bool> RetrieveModuleAsync(string ModuleName, string WebsiteName);
        public Task<bool> RetrieveAllModuleAsync(string WebsiteName);
        public Task<bool> UpdateModuleAsync(string ModuleName, string WebsiteName);
        public Task<bool> DeleteModuleAsync(string ModuleName, string WebsiteName);
        //table map
        public Task<bool> CreateTableAsync(string TableName, string ModuleName);
        public Task<bool> RetrieveTableAsync(string TableName, string ModuleName);
        public Task<CataloguesViewModel> RetrieveAllTableAsync(int szDatabaseMasterId);
        public Task<bool> UpdateTableAsync(string TableName, string ModuleName);
        public Task<bool> DeleteTableAsync(string TableName, string ModuleName);
        //Column map
        public Task<bool> CreateColumnAsync(string ModuleName, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> RetrieveColumnAsync(string ModuleName, string TableName, string ColumnName);
        public Task<bool> RetrieveAllColumnAsync(string ModuleName, string TableName);
        public Task<bool> UpdateColumnAsync(string ModuleName, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint);
        public Task<bool> DeleteColumnAsync(string ModuleName, string TableName, string ColumnName);
        // data map
        public Task<bool> InsertDataAsync(string ModuleName, string TableName, Dictionary<string, string> Data);
        public Task<bool> RetrieveDataAsync(string ModuleName, string TableName, string filter);
        public Task<bool> RetrieveAllDataAsync(string ModuleName, string TableName);
        public Task<bool> UpdateDataAsync(string ModuleName, string TableName, Dictionary<string, string> Data);
        public Task<bool> DeleteDataAsync(string ModuleName, string TableName, string DataId);
    }
}
