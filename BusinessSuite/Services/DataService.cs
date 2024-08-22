using BusinessSuite.Controllers;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<bool> DeleteDataAsync(string szConnectionString, string TableName, int DataId)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                string insertDataQuery = $"DELETE FROM {TableName} WHERE Id = {DataId}";

                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                    var insertedId = await command.ExecuteScalarAsync();


                    return true; // Assuming you want to return the inserted ID from your method
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                 await _connection.CloseAsync();
            }
        }
        public async Task<bool> DeleteAllDataAsync(string szConnectionString, string TableName)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                string insertDataQuery = $"DELETE FROM {TableName}";

                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                    var insertedId = await command.ExecuteScalarAsync();


                    return true; // Assuming you want to return the inserted ID from your method
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                 await _connection.CloseAsync();
            }
        }  
        public async Task<bool> DatabaseResetAsync(string szConnectionString)
        {
            try
            {
                //_connection = new SqlConnection(szConnectionString);
                //await _connection.OpenAsync();

                //string insertDataQuery = $"DELETE FROM {TableName}";

                //using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                //{
                //    var insertedId = await command.ExecuteScalarAsync();


                //    return true; // Assuming you want to return the inserted ID from your method
                //}


                using (var connection = new SqlConnection(szConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        // SQL command to drop all tables
                        command.CommandText = @"
                            DECLARE @sql NVARCHAR(MAX) = N'';
                            SELECT @sql += 'DROP TABLE [' + SCHEMA_NAME(schema_id) + '].[' + name + '];'
                            FROM sys.tables;
                            EXEC sp_executesql @sql;
                        ";
                        await command.ExecuteNonQueryAsync();

                        // Add any custom logic to recreate tables or schema here
                        // Alternatively, run your migrations if EF Core is still being used
                        command.CommandText = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';";
                        await command.ExecuteNonQueryAsync();
                    }
                }


                return true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                 await _connection.CloseAsync();
            }
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

                string insertDataQuery = $@"";
               
                {
                     insertDataQuery = $@"
                INSERT INTO {TableName} ({Columns}, CreatedDate) 
                VALUES ({ColumnValues}, '{DateTime.Now}');
                SELECT SCOPE_IDENTITY();";
                }

                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                    var insertedId = await command.ExecuteScalarAsync();



                    // Check if insertedId is null and return 0 if it is
                    if (insertedId == null || insertedId == DBNull.Value)
                    {
                        return 0;
                    }

                    return int.Parse(insertedId.ToString());
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                 await _connection.CloseAsync();
            }
        }
        public async Task<int> UploadDataAsync(string szConnectionString, string TableName, System.Data.DataTable dataTable)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                foreach (DataRow row in dataTable.Rows)
                {
                    var columnNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    var parameters = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => $"@{c.ColumnName}"));

                    var insertCommandText = $"INSERT INTO {TableName} ({columnNames},CreatedDate) VALUES ({parameters},'{DateTime.Now}')";


                    using (var command = new SqlCommand(insertCommandText, _connection))
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            command.Parameters.AddWithValue($"@{column.ColumnName}", row[column.ColumnName]);
                        }

                        // Using SqlDataAdapter to execute the command
                        using (var adapter = new SqlDataAdapter())
                        {
                            adapter.InsertCommand = command;
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                 await _connection.CloseAsync();
            }
        }

        public async Task<System.Data.DataTable> RetrieveAllColumnAsync(string szConnectionString, string TableName)
        {
            try
            {
                System.Data.DataTable columnSchema = new System.Data.DataTable();
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
                 await _connection.CloseAsync();
            }
        }

        public async Task<System.Data.DataTable> RetrieveAllDataAsync(string szConnectionString, string TableName)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                System.Data.DataTable tableSchema = new System.Data.DataTable();
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
                 await _connection.CloseAsync();
            }
        }
        public async Task<System.Data.DataTable> RunCustomQueryAsync(string szConnectionString, string szQuery)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                System.Data.DataTable tableSchema = new System.Data.DataTable();
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
                 await _connection.CloseAsync();
            }
        }

        public Task<bool> RetrieveAllModuleAsync(string WebsiteName)
        {
            throw new NotImplementedException();
        }

        public async Task<CataloguesViewModel> RetrieveAllTableAsync(System.String szConnectionString)
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
                 await _connection.CloseAsync();
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
                 await _connection.CloseAsync();
            }
        }
        public async Task<System.Data.DataTable> RetrieveAllTableReferencesAsync(string szConnectionString, string sourceTable, string targetTable)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();

                System.Data.DataTable columnSchemaDetail = new System.Data.DataTable();
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

        public async Task<bool> UpdateDataAsync(string szConnectionString, int rowId, string TableName, Dictionary<string, string> Data)
        {
            try
            {
                _connection = new SqlConnection(szConnectionString);
                await _connection.OpenAsync();
                var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");

                foreach (var column in Data)
                {
                    updateQuery.Append($"{column.Key} = @{column.Key}, ");
                }
                updateQuery.Length -= 2; // Remove the last comma
                updateQuery.Append($" WHERE Id = @Id");

                using (var command = new SqlCommand(updateQuery.ToString(), _connection))
                {
                    foreach (var column in Data)
                    {
                        command.Parameters.AddWithValue($"@{column.Key}", column.Value);
                    }
                    command.Parameters.AddWithValue("@Id", rowId);

                    await command.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               await _connection.CloseAsync();
            }
               
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
