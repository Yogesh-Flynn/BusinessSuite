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
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i=>i.Id==szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();


                TempData["DbMasterId"] = szDatabaseMasterId;






                /*DECLARE @TableA NVARCHAR(128) = 'Campaigns'; -- Replace with your first table name
DECLARE @TableB NVARCHAR(128) = 'Campaign_Customers'; -- Replace with your second table name

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
    AND PKCU.TABLE_NAME = @TableA;
*/



                DataTable columnSchema = new DataTable();
                DataTable columnSchemaDetail = new DataTable();
                List<string> tableNames = new List<string>();
                // Query to get column names and data types for the specific table
                string getColumnNamesQuery = @"
                    SELECT COLUMN_NAME, DATA_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";

                // Query to get all table names
                string getTableNamesQuery = @"
            SELECT TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE = 'BASE TABLE'
            ORDER BY TABLE_NAME";

                // Fetch column names for the specified table
                using (SqlCommand command = new SqlCommand(getColumnNamesQuery, _connection))
                {
                    command.Parameters.AddWithValue("@TableName", szTableName);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchema);
                    }
                }

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




                //////////////////////////
                ///
                foreach (var item in tableNames)
                {
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
                        command.Parameters.AddWithValue("@TableA", szTableName);
                        command.Parameters.AddWithValue("@TableB", item);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(columnSchemaDetail);
                        }
                    }

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
                            DataTable tableSchema = new DataTable();
                            try
                            {
                                string createTableQuery = @$"SELECT Id,Name,
                                       ROW_NUMBER() OVER (ORDER BY Id) AS RowNum 
                                       FROM {table}";

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
                                       FROM {table}";

                                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                                {

                                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                    {
                                        adapter.Fill(tableSchema);
                                    }
                                }
                            }


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
                            DataTable tableSchema = new DataTable();
                            try
                            {
                                string createTableQuery = @$"SELECT Id,Name,
                                       ROW_NUMBER() OVER (ORDER BY Id) AS RowNum 
                                       FROM {referencedtable}";

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
                                       FROM {referencedtable}";

                                using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                                {

                                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                    {
                                        adapter.Fill(tableSchema);
                                    }
                                }
                            }
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









                ViewBag.TableNames = tableNames;
                ViewBag.ColumnNames = columnSchema;

                ViewBag.TableName = szTableName;
                ViewData["TableName"] = szTableName;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching table data.");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                _connection.Close();
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

        [HttpGet]
        public async Task<IActionResult> GetTableData(int szDatabaseMasterId, string szTableName, string szColumnName = "id", int szPageIndex = 0, int szPageSize = 10)
        {
            try
            {

                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();
                TempData["DbMasterId"] = szDatabaseMasterId;

                DataTable columnSchema = new DataTable();
                DataTable columnSchemaDetail = new DataTable();
                DataTable columnSchemaDetail1 = new DataTable();
                List<string> tableNames = new List<string>();
                // Query to get column names and data types for the specific table
                string getColumnNamesQuery = @"
                    SELECT COLUMN_NAME, DATA_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";

                // Query to get all table names
                string getTableNamesQuery = @"
            SELECT TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE = 'BASE TABLE'
            ORDER BY TABLE_NAME";

                // Fetch column names for the specified table
                using (SqlCommand command = new SqlCommand(getColumnNamesQuery, _connection))
                {
                    command.Parameters.AddWithValue("@TableName", szTableName);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchema);
                    }
                }

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




                //////////////////////////
                ///
                foreach (var item in tableNames)
                {
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
                        command.Parameters.AddWithValue("@TableA", szTableName);
                        command.Parameters.AddWithValue("@TableB", item);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(columnSchemaDetail);
                        }
                    }

                }


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
                            columnSchema.Rows.Add(table, "int");

                        }
                    }
                    else
                    {
                        if(columnSchemaDetail.Rows.Count>1 && !referencedtable.Equals(szTableName))
                        {
                            columnSchema.Rows.Add(referencedtable, "int");
                        }
                        //DeleteRowByName(columnSchema, referencingColumn);
                        //columnSchema.Rows.Add(referencedtable, "int");
                    }
                }



                DataTable columnSchema1 = new DataTable();
                // Query to get column names and data types for the specific table
                string getColumnNamesQuery1 = @"
                    SELECT COLUMN_NAME, DATA_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";



                // Fetch column names for the specified table
                using (SqlCommand command = new SqlCommand(getColumnNamesQuery1, _connection))
                {
                    command.Parameters.AddWithValue("@TableName", szTableName);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchema1);
                    }
                }

                Dictionary<string, string> remaingdata1 = new Dictionary<string, string>();
                List<string> one = new List<string>();
                List<string> two = new List<string>();
                foreach (DataRow row in columnSchema.Rows)
                {
                    one.Add(row["COLUMN_NAME"].ToString());
                }
                foreach (DataRow row1 in columnSchema1.Rows)
                {
                    two.Add(row1["COLUMN_NAME"].ToString());

                }



                var nonMatchingFromOne = one.Except(two).ToList();

                // Get items that are in list 'two' but not in list 'one'
                var nonMatchingFromTwo = two.Except(one).ToList();

                // Combine both results
                var nonMatching = nonMatchingFromOne.Concat(nonMatchingFromTwo).ToList();

                List<string> thirdtable = new List<string>();
                foreach (DataRow row1 in columnSchemaDetail.Rows)
                {
                    var cool = row1["ReferencingTable"].ToString();
                    foreach (var item in nonMatching)
                    {
                        if (cool.Contains(szTableName) && cool.Contains(item))
                        {
                            thirdtable.Add(cool);
                        }

                    }

                }

                //////////////////////////
                ///
                foreach (var item in tableNames)
                {
                    foreach (var item2 in thirdtable)
                    {
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
                            command.Parameters.AddWithValue("@TableA", item2);
                            command.Parameters.AddWithValue("@TableB", item);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(columnSchemaDetail1);
                            }
                        }
                    }
                }
                ///////////////////////////////////////////////////
                ///
                // Assuming connection is already initialized


                DataTable tableSchema1 = new DataTable();

                var displaycols = "";
                var displaygroupby= "";
                int cnt = 0;
                foreach (var item in nonMatching)
                {
                    if (thirdtable.Count == 1)
                    {
                        if (thirdtable[0].Contains(item))
                        {

                            displaycols += $"m.Id,m.Name,STRING_AGG(p{cnt}.Name, ', ') AS {item},";
                            displaygroupby += $"GROUP BY m.Id, m.Name,";

                        }
                        else
                        {
                            displaycols += $"p{cnt}.Name AS {item}Name,";
                            displaygroupby += $"p{cnt}.Name";
                        }
                    }
                    else
                    {
                        displaycols += $"p{cnt}.Name AS {item}Name,";
                    }
                    cnt++;
                }
                var leftjoin = "";
                cnt = 0;


                /*  SELECT 
        m.Id,
        m.Name,
        m.Description,
        m.ProductId,
        STRING_AGG(p0.Name, ', ') AS Customers,
        p1.Name AS ProductsName,
        ROW_NUMBER() OVER (ORDER BY m.Id) AS RowNum 
    FROM 
        Marketings m
    LEFT JOIN 
        Marketings_Customers mp0 ON m.Id = mp0.MarketingId
    LEFT JOIN 
        Customers p0 ON mp0.CustomersId = p0.Id 
    LEFT JOIN 
        Products p1 ON m.ProductId = p1.Id
    GROUP BY 
        m.Id, m.Name, m.Description, m.ProductId, p1.Name*/

                foreach (var item in nonMatching)
                {
                    if (thirdtable.Count == 0)
                    {
                    }
                    else
                    {
                        if (thirdtable.Count == 1)
                        {
                            if (thirdtable[0].Contains(item))
                            {
                                leftjoin += $@" LEFT JOIN 
                                        {thirdtable[0]} mp{cnt} ON m.Id = mp{cnt}.{szTableName + "Id"}
                                         LEFT JOIN 
                                        {item} p{cnt} ON mp{cnt}.{item + "Id"} = p{cnt}.Id";
                            }
                            else
                            {
                                leftjoin += $@" 
                                             LEFT JOIN 
                                            {item} p{cnt} ON m.{item + "Id"} = p{cnt}.Id";
                            }
                        }
                        else
                        {
                            leftjoin += $@" LEFT JOIN 
                                        {thirdtable[cnt]} mp{cnt} ON m.Id = mp{cnt}.{szTableName + "Id"}
                                         LEFT JOIN 
                                        {item} p{cnt} ON mp{cnt}.{item + "Id"} = p{cnt}.Id";

                        }
                    }
                    cnt++;
                }
                string createTableQuery1 = "";
                if (displaygroupby.EndsWith(","))
                {
                    // Remove the comma from the end
                    displaygroupby = displaygroupby.TrimEnd(',');
                }

                if (displaygroupby.Equals(""))
                {
                    createTableQuery1 = @$"
                        SELECT * FROM (
                             SELECT *,
                     ROW_NUMBER() OVER (ORDER BY m.Id) AS RowNum 
                 FROM 
                     {szTableName} m
                {leftjoin}
                        ) AS SubQuery 
                        WHERE 
                            RowNum > (@PageIndex * @PageSize) AND RowNum <= ((@PageIndex + 1) * @PageSize)";
                }
                else
                {
                     createTableQuery1 = @$"
            SELECT * FROM (
                 SELECT {displaycols}
         ROW_NUMBER() OVER (ORDER BY m.Id) AS RowNum 
     FROM 
         {szTableName} m
    {leftjoin} {displaygroupby}
            ) AS SubQuery 
            WHERE 
                RowNum > (@PageIndex * @PageSize) AND RowNum <= ((@PageIndex + 1) * @PageSize)";
                }
                using (SqlCommand command = new SqlCommand(createTableQuery1, _connection))
                {
                    command.Parameters.AddWithValue("@PageIndex", szPageIndex);
                    command.Parameters.AddWithValue("@PageSize", szPageSize);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tableSchema1);
                    }
                }

                var jsonData = DataTableToJson(tableSchema1);


                /////////////////////////////////////////////////////
                //DataTable tableSchema = new DataTable();
                //string createTableQuery = @$"SELECT * FROM (
                //                       SELECT *,
                //                       ROW_NUMBER() OVER (ORDER BY {szColumnName}) AS RowNum 
                //                       FROM {szTableName}) AS SubQuery WHERE RowNum > 
                //                       (@PageIndex * @PageSize) AND RowNum<=((@PageIndex+1)*@PageSize)";

                //using (SqlCommand command = new SqlCommand(createTableQuery, _connection))
                //{
                //    command.Parameters.AddWithValue("@PageIndex", szPageIndex);
                //    command.Parameters.AddWithValue("@PageSize", szPageSize);

                //    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                //    {
                //        adapter.Fill(tableSchema);
                //    }
                //}

                //var jsonData = DataTableToJson(tableSchema);
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching table data.");
                return RedirectToAction("Error");
            }
            finally
            {
                _connection.Close();
            }
        }

        private List<Dictionary<string, object>> DataTableToJson(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    var rwdata = row[col];

                    if (rwdata != null && !string.IsNullOrWhiteSpace(rwdata.ToString()))
                    {
                        dict[col.ColumnName] = rwdata;
                    }
                    else
                    {
                        dict[col.ColumnName] = "-";
                    }

                }
                list.Add(dict);
            }
            return list;
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
        public async Task<IActionResult> AddData(string tableName, Dictionary<string, string> data)
        {
            try
            {

                int? szDatabaseMasterId = TempData["DbMasterId"] as int?;
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                _connection = new SqlConnection(sqlConnectionString.ConnectionString);
                _connection.Open();
                TempData["DbMasterId"] = szDatabaseMasterId;

                DataTable columnSchema = new DataTable();
                // Query to get column names and data types for the specific table
                string getColumnNamesQuery = @"
                    SELECT COLUMN_NAME, DATA_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";



                // Fetch column names for the specified table
                using (SqlCommand command = new SqlCommand(getColumnNamesQuery, _connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(columnSchema);
                    }
                }

                Dictionary<string, string> remaingdata = new Dictionary<string, string>();

                foreach (DataRow row in columnSchema.Rows)
                {
                    var columnName = row["COLUMN_NAME"].ToString();
                    if (data.ContainsKey(columnName))
                    {
                        remaingdata.Add(columnName, data[columnName]);
                        data.Remove(columnName);
                    }
                    else
                    {
                        if (!columnName.Equals("Id") && !columnName.Equals("CreatedDate"))
                        {
                            remaingdata.Add(columnName, data[columnName]);


                            data.Remove(columnName);
                        }

                    }
                }
                List<(string, string)> values1 = new List<(string, string)>();
                Dictionary<string, string> thirddata = new Dictionary<string, string>();
                foreach (var item in data)
                {
                    string sztablename = item.Key.ToString();
                    string[] table = sztablename.Split('~');

                    values1.Add((table[1], item.Value));
                }


                Dictionary<string, string> insertedData = new Dictionary<string, string>();

                /////////////////////////////////////////////////////////////
                ///



                string columns = string.Join(", ", remaingdata.Keys);
                string values = string.Join(", ", remaingdata.Values.Select(v => $"'{v}'"));
                //string insertDataQuery = $@"
                //INSERT INTO {tableName} ({columns}) 
                //VALUES ({values});
                //SELECT SCOPE_IDENTITY();";

                // Split the input string
                string output = $"";
                if (values.Contains(':'))
                {
                    string[] parts = values.Split(',');

                    foreach (var x in parts)
                    {
                        if (x.Contains(":"))
                        {

                           

                            string time = x.Replace("'","");
                            time = time.Trim();
                            // Parse the input string into a DateTime object
                            DateTime dateTime = DateTime.ParseExact(time, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                            // Format the DateTime object to the desired string format
                            string formattedTime = dateTime.ToString("M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);

                            Console.WriteLine(formattedTime);
                            output += "'" + formattedTime + "'" + ",";
                        }
                        else
                        {
                            output += x + ",";
                        }
                    }



                    values = output.Substring(0, output.Length - 1);

                }

                string campaignId = "";


                string insertDataQuery = $@"
                INSERT INTO {tableName} ({columns}, CreatedDate) 
                VALUES ({values}, '{DateTime.Now}');
                SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(insertDataQuery, _connection))
                {
                    var insertedId = await command.ExecuteScalarAsync();
                    insertedData.Add(tableName, insertedId.ToString());
                    campaignId=insertedId.ToString();
                    //return insertedId; // Assuming you want to return the inserted ID from your method
                }
             
                foreach (var entry in values1)
                {
                    insertedData.Add(entry.Item1, entry.Item2);
                    ////////////////////////////////////////
                    ///
                    List<string> tableNames = new List<string>();

                    DataTable columnSchemaDetail = new DataTable();
                    // Query to get all table names
                    string getTableNamesQuery = @"
                        SELECT TABLE_NAME
                        FROM INFORMATION_SCHEMA.TABLES
                        WHERE TABLE_TYPE = 'BASE TABLE'
                        ORDER BY TABLE_NAME";



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




                    //////////////////////////
                    ///
                    foreach (var item in tableNames)
                    {
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
                            command.Parameters.AddWithValue("@TableA", entry.Item1);
                            command.Parameters.AddWithValue("@TableB", item);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(columnSchemaDetail);
                            }
                        }

                    }
                    foreach (var item in tableNames)
                    {
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
                            command.Parameters.AddWithValue("@TableA", tableName);
                            command.Parameters.AddWithValue("@TableB", item);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(columnSchemaDetail);
                            }
                        }

                    }

                    string cols = "";
                    string paramsq = "";
                    string newtabname = "";
                    ////////////////////////////////////////
                    foreach (DataRow row in columnSchemaDetail.Rows)
                    {
                        var referencingtable = row["ReferencingTable"].ToString();
                        var referencingColumn = row["referencingColumn"].ToString();
                        var referencedtable = row["ReferencedTable"].ToString();
                        var referencedColumn = row["ReferencedColumn"].ToString();

                        if (referencingtable.Contains(entry.Item1) && referencingtable.Contains(tableName))
                        {
                            newtabname = referencingtable;
                            cols = cols + "," + referencingColumn;
                            paramsq = paramsq + "," + insertedData[referencedtable];
                        }


                    }
                    cols = cols.Substring(1);
                    paramsq = paramsq.Substring(1);


                    string insertDataQuery1 = $@"
                INSERT INTO {newtabname} ({cols}) 
                VALUES ({paramsq});
                SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertDataQuery1, _connection))
                    {
                        var insertedId = await command.ExecuteScalarAsync();
                        //return insertedId; // Assuming you want to return the inserted ID from your method
                    }
                    /////////////////////////////////////////////
                    ///
                    if (tableName.Equals("Campaigns"))
                    {


                        string query = @$"SELECT 
                            m.Id AS MarketingId,
                            c.Id AS CampaignId,
					        c.ScheduledDate as ScheduledDate,
                            m.Message as Message,
                            p0.PhoneNumber as PhoneNumber,
                            p0.Name AS CustomerName,
                            ROW_NUMBER() OVER (ORDER BY m.Id) AS RowNum 
                        FROM 
                            Marketings m
                        LEFT JOIN 
                            Marketings_Customers mp0 ON m.Id = mp0.MarketingsId
                        LEFT JOIN 
                            Customers p0 ON mp0.CustomersId = p0.Id 
                        LEFT JOIN 
                            Campaigns_Marketings cm ON m.Id = cm.MarketingsId
                        LEFT JOIN 
                            Campaigns c ON cm.CampaignsId = c.Id
                        WHERE 
                            c.Id = {campaignId}
                                         ";
                        DataTable dataTable = new DataTable();
                        using (SqlCommand command = new SqlCommand(query, _connection))
                        {
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }

                        foreach (DataRow row in dataTable.Rows)
                        {
                            var PhoneNumber = row["PhoneNumber"].ToString();
                            var Message = row["Message"].ToString();
                            var ScheduledDate = row["ScheduledDate"].ToString();
                            string scheduledDateString = row["ScheduledDate"].ToString();
                            DateTime scheduledDate;

                            if (DateTime.TryParse(scheduledDateString, out scheduledDate))
                            {
                                // scheduledDate now contains the parsed date
                                Console.WriteLine("Parsed Date: " + scheduledDate);
                            }
                           // BackgroundJob.Schedule(() => _jobService.StoreDataAsync(PhoneNumber, Message, "Pending", DateTime.Now), scheduledDate - DateTime.Now);
                        }
                        //BackgroundJob.Schedule(() => _jobService.StoreDataAsync(request.PhoneNumber, request.MessageText, request.Status, DateTime.Now), request.ScheduleTime - DateTime.Now);

                    }
                    insertedData.Remove(entry.Item1);
                }

              
                return RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId= szDatabaseMasterId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the data.");
                return BadRequest();//RedirectToAction("DisplayTable", new { szTableName = tableName, szDatabaseMasterId= szDatabaseMasterId });
            }
            finally
            {
                _connection.Close();
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
