using BusinessSuite.Controllers;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BusinessSuite.Services
{
    public class DataService : IDataService
    {
        private SqlConnection _connection;
        public DataService() { 
            
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

        public async Task<int> InsertDataAsync(string szConnectionString, string TableName, string Columns, string ColumnValues)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                string insertDataQuery = $@"
                INSERT INTO {TableName} ({Columns}, CreatedDate) 
                VALUES ({ColumnValues}, '{DateTime.Now}');
                SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                    var insertedId = await command.ExecuteScalarAsync();
                   
                    
                    return int.Parse(insertedId.ToString()); // Assuming you want to return the inserted ID from your method
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<DataTable> RetrieveAllColumnAsync(string szConnectionString, string TableName)
        {
            try
            {
                DataTable columnSchema = new DataTable();
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                string getColumnNamesQuery = @"
                    SELECT COLUMN_NAME, DATA_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";

                using (SqlCommand command = new SqlCommand(getColumnNamesQuery, _connection))
                {
                    command.Parameters.AddWithValue("@TableName", TableName);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchema);
                    }
                }

                return columnSchema;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<DataTable> RetrieveAllDataAsync(string szConnectionString, string TableName)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                DataTable tableSchema = new DataTable();
                try
                {
                    string createTableQuery = @$"SELECT Id,Name,
                                       ROW_NUMBER() OVER (ORDER BY Id) AS RowNum 
                                       FROM {TableName}";

                    using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                    {

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(tableSchema);
                        }
                    }
                }
                catch
                {
                    string createTableQuery = @$"SELECT *,
                                       ROW_NUMBER() OVER (ORDER BY Id) AS RowNum 
                                       FROM {TableName}";

                    using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                    {

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(tableSchema);
                        }
                    }
                }

                return tableSchema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<DataTable> RunCustomQueryAsync(string szConnectionString, string szQuery)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                DataTable tableSchema = new DataTable();
                try
                {
                    

                    using (SqlCommand command = new SqlCommand(szQuery, _connection))
                    {

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(tableSchema);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                return tableSchema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public Task<bool> RetrieveAllModuleAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public async Task<CataloguesViewModel> RetrieveAllTableAsync(String szConnectionString)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                // Use a concise way to create the query
                string createTableQuery = "SELECT * FROM sys.tables";

                // Create a list directly without the need for a DataTable
                List<Catalogues> tableNames = new List<Catalogues>();

                // Use a using statement for SqlCommand and SqlDataAdapter to ensure proper resource disposal
                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                    // Use ExecuteReaderAsync to execute the command asynchronously
                    using (SqlDataReader reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Use reader.GetString to get the table name directly
                            string tableName = reader.GetString(0);
                            DateTime datecreated = reader.GetDateTime(7);
                            Catalogues catalogues = new Catalogues();
                            catalogues.Name = tableName;
                            catalogues.CreatedDate = datecreated;
                            tableNames.Add(catalogues);
                        }
                    }
                }

                // Use object initializer syntax for TableNameViewModel
                var tableNameViewModel = new CataloguesViewModel
                {
                    Catalogues = tableNames
                };
                return tableNameViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<List<string>> RetrieveAllTableNameAsync(string szConnectionString)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                List<string> tableNames = new List<string>();
                string getTableNamesQuery = @"SELECT TABLE_NAME
                                                FROM INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_TYPE = 'BASE TABLE'
                                                ORDER BY TABLE_NAME";

                // Fetch column names for the specified table


                // Fetch all table names
                using (SqlCommand command = new SqlCommand(getTableNamesQuery, _connection))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        await _connection.OpenAsync();
                    }
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableNames.Add(reader.GetString(0));
                        }
                    }
                }
                return tableNames;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<DataTable> RetrieveAllTableReferencesAsync(string szConnectionString, string sourceTable, string targetTable)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                DataTable columnSchemaDetail = new DataTable();
                string getColumnDetailsQuery = @"
                                                        -- Check if TableA has a foreign key referencing TableB
                                                        SELECT 
                                                            FKCU.TABLE_NAME AS ReferencingTable,
                                                            FKCU.COLUMN_NAME AS ReferencingColumn,
                                                            PKCU.TABLE_NAME AS ReferencedTable,
                                                            PKCU.COLUMN_NAME AS ReferencedColumn
                                                        FROM 
                                                            INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                                                        INNER JOIN 
                                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE FKCU ON RC.CONSTRAINT_NAME = FKCU.CONSTRAINT_NAME
                                                        INNER JOIN 
                                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE PKCU ON RC.UNIQUE_CONSTRAINT_NAME = PKCU.CONSTRAINT_NAME
                                                        WHERE 
                                                            FKCU.TABLE_NAME = @TableA
                                                            AND PKCU.TABLE_NAME = @TableB

                                                        UNION

                                                        -- Check if TableB has a foreign key referencing TableA
                                                        SELECT 
                                                            FKCU.TABLE_NAME AS ReferencingTable,
                                                            FKCU.COLUMN_NAME AS ReferencingColumn,
                                                            PKCU.TABLE_NAME AS ReferencedTable,
                                                            PKCU.COLUMN_NAME AS ReferencedColumn
                                                        FROM 
                                                            INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                                                        INNER JOIN 
                                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE FKCU ON RC.CONSTRAINT_NAME = FKCU.CONSTRAINT_NAME
                                                        INNER JOIN 
                                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE PKCU ON RC.UNIQUE_CONSTRAINT_NAME = PKCU.CONSTRAINT_NAME
                                                        WHERE 
                                                            FKCU.TABLE_NAME = @TableB
                                                            AND PKCU.TABLE_NAME = @TableA;";
                using (SqlCommand command = new SqlCommand(getColumnDetailsQuery, _connection))
                {
                    command.Parameters.AddWithValue("@TableA", sourceTable);
                    command.Parameters.AddWithValue("@TableB", targetTable);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchemaDetail);
                    }
                }
                return columnSchemaDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.CloseAsync();
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
