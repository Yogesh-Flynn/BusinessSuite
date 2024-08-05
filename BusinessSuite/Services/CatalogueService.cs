using BusinessSuite.Data;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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


        public Task<bool> CreateColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateModuleAsync(int szDatabaseMasterId, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTableAsync(string TableName, int szDatabaseMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDataAsync(int szDatabaseMasterId, string TableName, string DataId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteModuleAsync(int szDatabaseMasterId, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTableAsync(string TableName, int szDatabaseMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertDataAsync(int szDatabaseMasterId, string TableName, Dictionary<string, string> Data)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTable> RetrieveAllColumnAsync(int szDatabaseMasterId, string TableName)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var columnSchema = await _dataService.RetrieveAllColumnAsync(sqlConnectionString.ConnectionString, TableName);
                return columnSchema;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> RetrieveAllDataAsync(int szDatabaseMasterId, string TableName)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var tabledata = await _dataService.RetrieveAllDataAsync(sqlConnectionString.ConnectionString, TableName);
                return tabledata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            }
            catch(Exception ex)
            {
                throw ex;
            }
           

        } 
        
        public async Task<DisplayTableViewModel> DisplayTableAsync(int szDatabaseMasterId, string szTableName)
        {
            try
            {
                
                var columnSchema = await RetrieveAllColumnAsync(szDatabaseMasterId, szTableName);

                var tableNames =await RetrieveAllTableNameAsync(szDatabaseMasterId);

                DataTable columnSchemaDetail = new DataTable();

                //////////////////////////
                ///
                foreach (var item in tableNames)
                {
                    columnSchemaDetail.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, szTableName, item));

                }
                columnSchema.Columns.Add("coldata");

               

                ////////////////////////////////////////
                foreach (DataRow row in columnSchemaDetail.Rows)
                {
                    var referencingtable = row["ReferencingTable"].ToString();
                    var referencingColumn = row["referencingColumn"].ToString();
                    var referencedtable = row["ReferencedTable"].ToString();
                    var referencedColumn = row["ReferencedColumn"].ToString();

                    String[] tablename = referencingtable.Split('_');
                    if (tablename.Length > 1)
                    {
                        if (tablename[0].Contains(szTableName))
                        {
                            var table = tablename[1];
                            DataTable tableSchema = await RetrieveAllDataAsync(szDatabaseMasterId, table);
                           


                            String data = "";
                            foreach (DataRow item in tableSchema.Rows)
                            {
                                data = data + '~' + item["Name"].ToString() + "-" + item["Id"].ToString();
                            }
                            if (!data.Equals(""))
                            {
                                data = data.Substring(1);

                            }
                            columnSchema.Rows.Add(table, "manyselect", data);

                        }
                    }
                    else
                    {
                        if (!referencedtable.Equals(szTableName))
                        {
                            DataTable tableSchema= await RetrieveAllDataAsync(szDatabaseMasterId, referencedtable);                            

                            string namecol = "";
                            foreach (DataColumn dataColumn in tableSchema.Columns)
                            {
                                string colname = dataColumn.ColumnName;
                                if (colname.Contains("Name"))
                                {
                                    namecol = colname;
                                }
                            }
                            String data = "";
                            foreach (DataRow item in tableSchema.Rows)
                            {
                                data = data + '~' + item[namecol].ToString() + "-" + item["Id"].ToString();
                            }
                            if (!data.Equals(""))
                            {
                                data = data.Substring(1);

                            }
                            // Delete a row by Name
                            DeleteRowByName(columnSchema, referencingColumn);
                            columnSchema.Rows.Add(referencingColumn, "oneselect", data);
                            
                        }
                    }
                }


                /////////////////////////////////////////////////////
                return new DisplayTableViewModel()
                {
                    ColumnSchema = columnSchema,
                    DisplayTables = tableNames
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }
        void DeleteRowByName(DataTable table, string name)
        {
            DataRow rowToDelete = null;
            foreach (DataRow row in table.Rows)
            {
                if (row["COLUMN_NAME"].ToString() == name)
                {
                    rowToDelete = row;
                    break;
                }
            }

            if (rowToDelete != null)
            {
                table.Rows.Remove(rowToDelete);
            }
        }
        public async Task<List<string>> RetrieveAllTableNameAsync(int szDatabaseMasterId)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var result = await _dataService.RetrieveAllTableNameAsync(sqlConnectionString.ConnectionString);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<DataTable> RetrieveAllTableReferencesAsync(int szDatabaseMasterId, string sourceTable, string targetTable)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var result = await _dataService.RetrieveAllTableReferencesAsync(sqlConnectionString.ConnectionString, sourceTable, targetTable);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<bool> RetrieveAllWebsitesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveDataAsync(int szDatabaseMasterId, string TableName, string filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveModuleAsync(int szDatabaseMasterId, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveTableAsync(string TableName, int szDatabaseMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RetrieveWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateColumnAsync(int szDatabaseMasterId, string TableName, string ColumnName, string ColumnDataType, bool isNull, string ColumnConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDataAsync(int szDatabaseMasterId, string TableName, Dictionary<string, string> Data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateModuleAsync(int szDatabaseMasterId, string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTableAsync(string TableName, int szDatabaseMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWebsiteAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }
    }
}
