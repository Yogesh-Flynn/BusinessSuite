using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Globalization;
using System.Text;

namespace BusinessSuite.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public CatalogueController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("customDBConn"));
            _connection.Open();
        }
        public IActionResult Index()
        {
            try
            {
                // Use a concise way to create the query
                string createTableQuery = "SELECT * FROM sys.tables";

                // Create a list directly without the need for a DataTable
                List<Catalogues> tableNames = new List<Catalogues>();

                // Use a using statement for SqlCommand and SqlDataAdapter to ensure proper resource disposal
                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                    // Use ExecuteReaderAsync to execute the command asynchronously
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Use reader.GetString to get the table name directly
                            string tableName = reader.GetString(0);
                            DateTime datecreated = reader.GetDateTime(7);
                            Catalogues catalogues = new Catalogues();
                            catalogues.Name = tableName;
                            catalogues.CreatedDate= datecreated;
                            tableNames.Add(catalogues);
                        }
                    }
                }

                // Use object initializer syntax for TableNameViewModel
                var tableNameViewModel = new CataloguesViewModel
                {
                    Catalogues = tableNames
                };

                // Return the view with the populated model
                return View(tableNameViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError(ex, "An error occurred while fetching table names.");

                // Handle the exception gracefully, show an error message, or redirect to an error page
                // You can customize this based on your application's needs.
                return RedirectToAction("Error");
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetTableData(string szTableName, string szColumnName = "id", int szPageIndex = 0, int szPageSize = 10)
        {
            try
            {
                DataTable tableSchema = new DataTable();
                // string createTableQuery = $"SELECT * FROM {szTableName} where ";
                string createTableQuery = @$"SELECT * FROM {szTableName}";

                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                    command.Parameters.AddWithValue("@PageIndex", szPageIndex);
                    command.Parameters.AddWithValue("@PageSize", szPageSize);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tableSchema);
                    }
                }
                //if (tableSchema.Columns.Contains("Id"))
                //{
                //    tableSchema.Columns.Remove("Id");
                //}
                //if (tableSchema.Columns.Contains("CreatedDate"))
                //{
                //    tableSchema.Columns.Remove("CreatedDate");
                //}
                ViewBag.TableName = szTableName;
                return View(tableSchema);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError(ex, "An error occurred while fetching table names.");

                // Handle the exception gracefully, show an error message, or redirect to an error page
                // You can customize this based on your application's needs.
                return RedirectToAction("Error");
            }

        }
        
        [HttpGet]
        public async Task<IActionResult> GetTableDataScroll(string szTableName, string szColumnName = "id", int szPageIndex = 0, int szPageSize = 10)
        {
            try
            {
                DataTable tableSchema = new DataTable();
                // string createTableQuery = $"SELECT * FROM {szTableName} where ";
                string createTableQuery = @$"SELECT * FROM (
                                               SELECT *,
                                                ROW_NUMBER() OVER (ORDER BY {szColumnName}) AS RowNum 
                                                FROM {szTableName}) AS SubQuery WHERE RowNum > 
                                                (@PageIndex * @PageSize) AND RowNum<=((@PageIndex+1)*@PageSize)";

                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                    command.Parameters.AddWithValue("@PageIndex", szPageIndex);
                    command.Parameters.AddWithValue("@PageSize", szPageSize);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tableSchema);
                    }
                }
                return Json(tableSchema);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError(ex, "An error occurred while fetching table names.");

                // Handle the exception gracefully, show an error message, or redirect to an error page
                // You can customize this based on your application's needs.
                return RedirectToAction("Error");
            }

        }

        [HttpGet]
        public IActionResult CreateTable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(string tableName)
        {
            try
            {
                string createTableQuery = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100), CreatedDate DATETIME)";

                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {
                   
                    await command.ExecuteNonQueryAsync();
                   
                }

                return RedirectToAction("Index"); // Redirect to the catalogues list after creating the table
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the table.");
                return View();
            }



        }

        [HttpGet]
        public IActionResult EditTable(string tableName)
        {
            ViewBag.TableName = tableName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(string oldTableName, string newTableName)
        {
            try
            {
                string renameTableQuery = $"EXEC sp_rename '{oldTableName}', '{newTableName}'";

                using (SqlCommand command = new SqlCommand(renameTableQuery, _connection))
                {
                    
                    await command.ExecuteNonQueryAsync();
                 
                }

                return RedirectToAction("Index"); // Redirect to the catalogues list after renaming the table
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while renaming the table.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult DeleteTable(string tableName)
        {
            ViewBag.TableName = tableName;
            return View();
        }

        [HttpPost, ActionName("DeleteTable")]
        public async Task<IActionResult> DeleteTableConfirmed(string tableName)
        {
            try
            {
                string dropTableQuery = $"DROP TABLE {tableName}";

                using (SqlCommand command = new SqlCommand(dropTableQuery, _connection))
                {
                    
                    await command.ExecuteNonQueryAsync();
                   
                }

                return RedirectToAction("Index"); // Redirect to the catalogues list after deleting the table
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the table.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddColumn(string tableName, string columnName, string columnType)
        {
            try
            {
                string addColumnQuery = $"ALTER TABLE {tableName} ADD {columnName} {columnType}";

                using (SqlCommand command = new SqlCommand(addColumnQuery, _connection))
                {
                   
                    await command.ExecuteNonQueryAsync();
                    
                }

                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the column.");
                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteColumn(string tableName, string columnName)
        {
            try
            {
                string deleteColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {columnName}";

                using (SqlCommand command = new SqlCommand(deleteColumnQuery, _connection))
                {
                   
                    await command.ExecuteNonQueryAsync();
                    
                }

                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the column.");
                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddData(string tableName, Dictionary<string, string> data)
        {
            try
            {
                string columns = string.Join(", ", data.Keys);
                string values = string.Join(", ", data.Values.Select(v => $"'{v}'"));
                string insertDataQuery = $"INSERT INTO {tableName} ({columns},CreatedDate) VALUES ({values},'{DateTime.Now}')";

                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                   
                    await command.ExecuteNonQueryAsync();
                    
                }

                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the data.");
                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteData(string tableName, int id)
        //{
        //    try
        //    {
        //        string deleteDataQuery = $"DELETE FROM {tableName} WHERE Id = {id}";

        //        using (SqlCommand command = new SqlCommand(deleteDataQuery, _connection))
        //        {
                   
        //            await command.ExecuteNonQueryAsync();
                    
        //        }

        //        return RedirectToAction("GetTableData", new { szTableName = tableName });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while deleting the data.");
        //        return RedirectToAction("GetTableData", new { szTableName = tableName });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateData(string tableName, int id, Dictionary<string, string> data)
        //{
        //    try
        //    {
        //        string setClause = string.Join(", ", data.Select(kvp => $"{kvp.Key} = '{kvp.Value}'"));
        //        string updateDataQuery = $"UPDATE {tableName} SET {setClause} WHERE Id = {id}";

        //        using (SqlCommand command = new SqlCommand(updateDataQuery, _connection))
        //        {
                   
        //            await command.ExecuteNonQueryAsync();
                    
        //        }

        //        return RedirectToAction("GetTableData", new { szTableName = tableName });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating the data.");
        //        return RedirectToAction("GetTableData", new { szTableName = tableName });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file,string tablename)
        {
            if (file == null || (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && file.ContentType != "text/csv"))
            {
                ModelState.AddModelError("", "Invalid file type. Please upload an Excel or CSV file.");
                return View("UploadFile", new DataTable());
            }

            var dataTable = new DataTable();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    if (file.FileName.EndsWith(".xlsx"))
                    {
                        using (var workbook = new XLWorkbook(stream))
                        {
                            var worksheet = workbook.Worksheet(1);
                            foreach (var cell in worksheet.Row(1).CellsUsed())
                            {
                                dataTable.Columns.Add(cell.Value.ToString());
                            }

                            foreach (var row in worksheet.RowsUsed().Skip(1))
                            {
                                var dataRow = dataTable.NewRow();
                                for (int i = 0; i < dataTable.Columns.Count; i++)
                                {
                                    dataRow[i] = row.Cell(i + 1).Value;
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var columnNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                        var parameters = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => $"@{c.ColumnName}"));
                        
                        var insertCommandText = $"INSERT INTO {tablename} ({columnNames},CreatedDate) VALUES ({parameters},'{DateTime.Now}')";
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

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the file.");
                ModelState.AddModelError("", "An error occurred while processing the file.");
                return View("UploadFile", new DataTable());
            }

            return View("UploadFile", dataTable);
        }

        [HttpGet]
        public IActionResult UploadFile()
        {
            return View(new DataTable());
        }
        [HttpGet]
        public async Task<IActionResult> DownloadTemplate(string tableName)
        {
            var dataTable = new DataTable();
            try
            {
               
                    var query = $"SELECT TOP 0 * FROM {tableName}";
                    using (var command = new SqlCommand(query, _connection))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Template");

                    // Add columns to the worksheet
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        var fileName = $"{tableName}_Template.xlsx";

                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the template.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditData(string tableName, int rowId, Dictionary<string, string> data)
        {
            try
            {
                var updateQuery = new StringBuilder($"UPDATE {tableName} SET ");

                foreach (var column in data)
                {
                    updateQuery.Append($"{column.Key} = @{column.Key}, ");
                }
                updateQuery.Length -= 2; // Remove the last comma
                updateQuery.Append($" WHERE Id = @Id");

                using (var command = new SqlCommand(updateQuery.ToString(), _connection))
                {
                    foreach (var column in data)
                    {
                        command.Parameters.AddWithValue($"@{column.Key}", column.Value);
                    }
                    command.Parameters.AddWithValue("@Id", rowId);

                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing data.");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteData(string tableName, int rowId)
        {
            try
            {
                var deleteQuery = $"DELETE FROM {tableName} WHERE Id = @Id";

                using (var command = new SqlCommand(deleteQuery, _connection))
                {
                    command.Parameters.AddWithValue("@Id", rowId);

                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction("GetTableData", new { szTableName = tableName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting data.");
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public IActionResult DeleteDataBulk(string tableName, List<int> selectedRows)
        {
            // Implement your logic to delete rows based on selectedRows IDs from the database.
            // ...

            return RedirectToAction("GetTableData", new { szTableName = tableName });
        }

    }
}
