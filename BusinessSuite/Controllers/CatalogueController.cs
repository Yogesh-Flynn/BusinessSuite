using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
                            Catalogues catalogues = new Catalogues();
                            catalogues.Name = tableName;
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
    }
}
