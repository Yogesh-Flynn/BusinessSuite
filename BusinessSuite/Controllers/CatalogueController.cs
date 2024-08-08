using Azure.Core;
using BusinessSuite.Data;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using BusinessSuite.Services;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using X.PagedList;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;

namespace BusinessSuite.Controllers
{

    public class CatalogueController : Controller
    {
        private readonly ICatalogueService _catalogueService;


        private readonly MyJobService _jobService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private  SqlConnection _connection;
        public CatalogueController(ICatalogueService catalogueService,MyJobService jobService,ILogger<HomeController> logger, IConfiguration configuration,ApplicationDbContext dbContext)
        {
            _jobService = jobService;
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _catalogueService = catalogueService;
            
        }
        public async Task<IActionResult> Index(int szDatabaseMasterId)
        {
            try
            {
                var tableNameViewModel = await _catalogueService.RetrieveAllTableAsync(szDatabaseMasterId);
                TempData["DbMasterId"] = szDatabaseMasterId;
                return View(tableNameViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching table names.");

                return RedirectToAction("Error");
            }
        }



        [HttpGet]
        public async Task<IActionResult> DisplayTable(string szTableName,int szDatabaseMasterId)
        {
            try
            {
                var result =await _catalogueService.DisplayTableAsync(szDatabaseMasterId,szTableName);

                TempData["DbMasterId"] = szDatabaseMasterId;


                ViewBag.TableNames = result.DisplayTables;
                ViewBag.ColumnNames = result.ColumnSchema;

                ViewBag.TableName = szTableName;
                ViewData["TableName"] = szTableName;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching table data.");
                return StatusCode(500, "Internal server error");
            }
         
        }
       

        [HttpGet]
        public async Task<IActionResult> GetTableData(int szDatabaseMasterId, string szTableName, string szColumnName = "id", int szPageIndex = 0, int szPageSize = 10)
        {
            try
            {

                var jsonData = await _catalogueService.RetrieveAllTableDataAsync(szDatabaseMasterId, szTableName);
                TempData["DbMasterId"] = szDatabaseMasterId;
               
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching table data.");
                return RedirectToAction("Error");
            }
         
        }

        [HttpGet]
        public IActionResult CreateTable(int szDatabaseMasterId)
        {
;
            TempData["DbMasterId"]= szDatabaseMasterId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(string tableName,int szDatabaseMasterId)
        {
            try
            {
                var dbmasterid = TempData["DbMasterId"];
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();


                TempData["DbMasterId"] = szDatabaseMasterId;

                string createTableQuery = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100), CreatedDate DATETIME)";

                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                {

                    await command.ExecuteNonQueryAsync();

                }

                return RedirectToAction("Index", new { szDatabaseMasterId }); // Redirect to the catalogues list after creating the table
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the table.");
                return View();
            }

            finally
            {
                _connection.Close();
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
        public async Task<IActionResult> AddColumn(int szDatabaseMasterId,string tableName, string columnName, string columnType, string referencedTable, string referencedColumn)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();


                TempData["DbMasterId"] = szDatabaseMasterId;
                string addColumnQuery;
                if (columnType == "foreign_key")
                {
                    // Create the column and add the foreign key constraint
                    addColumnQuery = $"ALTER TABLE {tableName} ADD {columnName} INT;" +
                                     $"ALTER TABLE {tableName} ADD CONSTRAINT FK_{tableName}_{columnName} FOREIGN KEY ({columnName}) REFERENCES {referencedTable}({referencedColumn})";
                }
                else
                {
                    // Add a regular column
                    addColumnQuery = $"ALTER TABLE {tableName} ADD {columnName} {columnType}";
                }

                using (SqlCommand command = new SqlCommand(addColumnQuery, _connection))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        await _connection.OpenAsync();
                    }
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId = szDatabaseMasterId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the column.");
                return RedirectToAction("DisplayTable", new { szTableName = tableName });
            }
            finally
            {
                _connection.Close();
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteColumn(int szDatabaseMasterId, string tableName, string columnName)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();


                TempData["DbMasterId"] = szDatabaseMasterId;

                string deleteColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {columnName}";

                using (SqlCommand command = new SqlCommand(deleteColumnQuery, _connection))
                {

                    await command.ExecuteNonQueryAsync();

                }

                return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId = szDatabaseMasterId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the column.");
                return RedirectToAction("DisplayTable", new { szTableName = tableName });
            }
            finally
            {
                _connection.Close();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddData(string tableName, Dictionary<string, string> data, int szDatabaseMasterId)
        {
            try
            {
                if (data == null || data.Values.All(string.IsNullOrEmpty))
                {

                    TempData["ErrorMessage"] = "The data dictionary is empty. Please provide data to insert.";
                    return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId = szDatabaseMasterId });

                }

                await _catalogueService.InsertDataAsync(szDatabaseMasterId, tableName, data);
                TempData["DbMasterId"] = szDatabaseMasterId;

                return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId = szDatabaseMasterId });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the data.");
                TempData["ErrorMessage"] = "An error occurred while adding the data.";
                return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId = szDatabaseMasterId });
        
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
        public async Task<IActionResult> UploadFile(IFormFile file, string tablename)
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

            return RedirectToAction("Index");
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

                return RedirectToAction("DisplayTable", new { szTableName = tableName });
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

                return RedirectToAction("DisplayTable", new { szTableName = tableName });
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

            return RedirectToAction("DisplayTable", new { szTableName = tableName });
        }

    }
}
