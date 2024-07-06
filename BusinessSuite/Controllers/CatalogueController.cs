using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

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
                   // await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                   // await _connection.CloseAsync();
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
                    //await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                   // await _connection.CloseAsync();
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
                    //await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                   // await _connection.CloseAsync();
                }

                return RedirectToAction("Index"); // Redirect to the catalogues list after deleting the table
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the table.");
                return View();
            }
        }
    }
}
