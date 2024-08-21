using BusinessSuite.Data;
using BusinessSuite.Interfaces;
using BusinessSuite.Models;
using BusinessSuite.Models.ViewModels;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Globalization;
using System.Text;

namespace BusinessSuite.Services
{
    public class CatalogueService : ICatalogueService
    {
        private readonly IDataService _dataService;

        private readonly MyJobService _jobService;
        private readonly ApplicationDbContext _dbContext;
        public CatalogueService(IDataService dataService, MyJobService jobService, ApplicationDbContext dbContext) 
        {
            _dataService = dataService;
            _dbContext = dbContext;

            _jobService = jobService;
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

        public async Task<bool> DeleteDataAsync(int szDatabaseMasterId, string TableName, int DataId)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();


               await _dataService.DeleteDataAsync(sqlConnectionString.ConnectionString, TableName, DataId);
               return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteAllDataAsync(int szDatabaseMasterId, string TableName)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();


                await _dataService.DeleteAllDataAsync(sqlConnectionString.ConnectionString, TableName);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DatabaseResetAsync(int szDatabaseMasterId)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();


                await _dataService.DatabaseResetAsync(sqlConnectionString.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public async Task<bool> UploadDataAsync(int szDatabaseMasterId, string TableName, IFormFile file)
        {

            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();


                var DataTable = new System.Data.DataTable();
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
                                    DataTable.Columns.Add(cell.Value.ToString());
                                }

                                foreach (var row in worksheet.RowsUsed().Skip(1))
                                {
                                    var dataRow = DataTable.NewRow();
                                    for (int i = 0; i < DataTable.Columns.Count; i++)
                                    {
                                        dataRow[i] = row.Cell(i + 1).Value;
                                    }
                                    DataTable.Rows.Add(dataRow);
                                }
                            }
                        }

                        await _dataService.UploadDataAsync(sqlConnectionString.ConnectionString, TableName, DataTable);

                    }

                }
                catch (Exception ex)
                {
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> InsertDataAsync(int szDatabaseMasterId, string TableName, Dictionary<string, string> data)
        {

            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                System.Data.DataTable columnSchema = await _dataService.RetrieveAllColumnAsync(sqlConnectionString.ConnectionString,TableName);


                Dictionary<string, string> remaingdata = new Dictionary<string, string>();

                foreach (DataRow row in columnSchema.Rows)
                {
                    var columnName = row["COLUMN_NAME"].ToString();
                    try
                    {
                        if (data.ContainsKey(columnName))
                        {
                            if (data[columnName] != null)
                            {
                                remaingdata.Add(columnName, data[columnName]);
                                data.Remove(columnName);
                            }
                            else
                            {
                                data.Remove(columnName);
                            }
                        }
                        else
                        {
                            if (!columnName.Equals("Id") && !columnName.Equals("CreatedDate"))
                            {
                                remaingdata.Add(columnName, data[columnName]);


                                data.Remove(columnName);
                            }

                        }
                    }catch(Exception ex)
                    {

                    }
                }

                List<(string, string)> values1 = new List<(string, string)>();
                Dictionary<string, string> thirddata = new Dictionary<string, string>();
                foreach (var item in data)
                {
                    string sztablename = item.Key.ToString();
                    string[] table = sztablename.Split('~');
                    if (table.Length > 1)
                    {
                        values1.Add((table[1], item.Value));

                    }
                }

                Dictionary<string, string> insertedData = new Dictionary<string, string>();
                string columns = string.Join(", ", remaingdata.Keys);
                string values = string.Join(", ", remaingdata.Values.Select(v => $"'{v}'"));
                /////////////////////////////////////////////////////////////
                ///
                string output = $"";
                if (values.Contains(':'))
                {
                    string[] parts = values.Split(',');

                    foreach (var x in parts)
                    {
                        if (x.Contains(":"))
                        {



                            string time = x.Replace("'", "");
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
                int campaignId = await _dataService.InsertDataAsync(sqlConnectionString.ConnectionString,TableName,columns,values);


                insertedData.Add(TableName, campaignId.ToString());
                foreach (var entry in values1)
                {
                    insertedData.Add(entry.Item1, entry.Item2);
                    ////////////////////////////////////////
                    ///
                    List<string> tableNames =await _dataService.RetrieveAllTableNameAsync(sqlConnectionString.ConnectionString);

                    System.Data.DataTable columnSchemaDetail = new System.Data.DataTable();
                    // Query to get all table names





                    //////////////////////////
                    ///
                    foreach (var item in tableNames)
                    {
                        columnSchemaDetail.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, entry.Item1, item));

                    }
                    foreach (var item in tableNames)
                    {
                        columnSchemaDetail.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, item, TableName));
                        //columnSchemaDetail.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, entry.Item1, TableName));
                       
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

                        if (referencingtable.Contains(entry.Item1) && referencingtable.Contains(TableName))
                        {
                            newtabname = referencingtable;
                            cols = cols + "," + referencingColumn;
                            paramsq = paramsq + "," + insertedData[referencedtable];
                        }


                    }
                    cols = cols.Substring(1);
                    paramsq = paramsq.Substring(1);

                    await _dataService.InsertDataAsync(sqlConnectionString.ConnectionString, newtabname, cols, paramsq);
                   
                    /////////////////////////////////////////////
                    ///
                    if (TableName.Equals("Campaigns"))
                    {
                        string query = @$"SELECT 
                            m.Id AS MarketingId,
                            m.TransitCarrier,
                            c.Id AS CampaignId,
					        c.ScheduledDate as ScheduledDate,
                            m.Message as Message,
                            m.Image as Image,
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
                        System.Data.DataTable DataTable = new System.Data.DataTable();

                        DataTable = await _dataService.RunCustomQueryAsync(sqlConnectionString.ConnectionString, query);
                       

                        foreach (DataRow row in DataTable.Rows)
                        {
                            var PhoneNumber = row["PhoneNumber"].ToString();
                            var Message = row["Message"].ToString();
                            var ScheduledDate = row["ScheduledDate"].ToString();
                            string scheduledDateString = row["ScheduledDate"].ToString();
                            string Image = row["Image"].ToString();
                            string TransitCarrier = row["TransitCarrier"].ToString();
                            DateTime scheduledDate;

                            if (DateTime.TryParse(scheduledDateString, out scheduledDate))
                            {
                                // scheduledDate now contains the parsed date
                                Console.WriteLine("Parsed Date: " + scheduledDate);
                            }
                             BackgroundJob.Schedule(() => _jobService.StoreDataAsync(PhoneNumber, TransitCarrier, Message,Image, "Pending", DateTime.Now), scheduledDate - DateTime.Now);
                        }
                        //BackgroundJob.Schedule(() => _jobService.StoreDataAsync(request.PhoneNumber, request.MessageText, request.Status, DateTime.Now), request.ScheduleTime - DateTime.Now);

                    }
                    insertedData.Remove(entry.Item1);
                }



                //////////////////////////////////////////////////////////////////
               
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

        public async Task<System.Data.DataTable> RetrieveAllColumnAsync(int szDatabaseMasterId, string TableName)
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

        public async Task<System.Data.DataTable> RetrieveAllDataAsync(int szDatabaseMasterId, string TableName)
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
        
        public async Task<List<Dictionary<string, object>>> RetrieveAllTableDataAsync(int szDatabaseMasterId, string TableName, string szColumnName = "id", int szPageIndex = 0, int szPageSize = 10)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                var tabledata = await _dataService.RetrieveAllDataAsync(sqlConnectionString.ConnectionString, TableName);
               // return tabledata;






                System.Data.DataTable columnSchema = await _dataService.RetrieveAllColumnAsync(sqlConnectionString.ConnectionString, TableName);

                List<string> tableNames = await _dataService.RetrieveAllTableNameAsync(sqlConnectionString.ConnectionString);


                System.Data.DataTable columnSchemaDetail = new System.Data.DataTable();
                System.Data.DataTable columnSchemaDetail1 = new System.Data.DataTable();



                //////////////////////////
                ///
                foreach (var item in tableNames)
                {
                    columnSchemaDetail.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, TableName, item));

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
                        if (tablename[0].Contains(TableName))
                        {
                            var table = tablename[1];
                            columnSchema.Rows.Add(table, "int");

                        }
                    }
                    else
                    {
                        if (columnSchemaDetail.Rows.Count > 1 && !referencedtable.Equals(TableName))
                        {
                            columnSchema.Rows.Add(referencedtable, "int");
                        }
                        //DeleteRowByName(columnSchema, referencingColumn);
                        //columnSchema.Rows.Add(referencedtable, "int");
                    }
                }



                System.Data.DataTable columnSchema1 = await _dataService.RetrieveAllColumnAsync(sqlConnectionString.ConnectionString, TableName); 
               

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
                        if (cool.Contains(TableName) && cool.Contains(item))
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
                        columnSchemaDetail1.Merge(await RetrieveAllTableReferencesAsync(szDatabaseMasterId, item2, item));
                    }
                }
                ///////////////////////////////////////////////////
                ///
                // Assuming connection is already initialized

                string maintablecols = "";
                foreach (var item in two)
                {
                    maintablecols += "m." + item+",";
                }
                System.Data.DataTable tableSchema1 = new System.Data.DataTable();

                var displaycols = "";
                var displaygroupby = "";
                int cnt = 0;
                foreach (var item in nonMatching)
                {
                    if (thirdtable.Count == 1)
                    {
                        if (thirdtable[0].Contains(item))
                        {

                            displaycols += $"{maintablecols}STRING_AGG(p{cnt}.Name, ', ') AS {item},";
                            displaygroupby += $"GROUP BY {maintablecols}";
                            maintablecols += "p1.Name";

                        }
                        else
                        {
                            displaycols += $"p{cnt}.Name AS {item}Name,";
                            displaygroupby += $"{maintablecols}";
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
                                        {thirdtable[0]} mp{cnt} ON m.Id = mp{cnt}.{TableName + "Id"}
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
                                        {thirdtable[cnt]} mp{cnt} ON m.Id = mp{cnt}.{TableName + "Id"}
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
                     {TableName} m
                {leftjoin}
                        ) AS SubQuery 
                        WHERE 
                            RowNum > ({szPageIndex} * {szPageSize}) AND RowNum <= (({szPageIndex} + 1) * {szPageSize})";
                }
                else
                {
                    createTableQuery1 = @$"
                                            SELECT * FROM (SELECT {displaycols} ROW_NUMBER() OVER (ORDER BY {maintablecols} ) AS RowNum 
                                            FROM 
                                            {TableName} m
                                            {leftjoin} {displaygroupby}) AS SubQuery 
                                            WHERE 
                                                RowNum > ({szPageIndex} * {szPageSize}) AND RowNum <= (({szPageIndex} + 1) * {szPageSize})";
                }
                tableSchema1 = await _dataService.RunCustomQueryAsync(sqlConnectionString.ConnectionString, createTableQuery1);
                return DataTableToJson(tableSchema1);


                /////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<Dictionary<string, object>> DataTableToJson(System.Data.DataTable table)
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

                System.Data.DataTable columnSchemaDetail = new System.Data.DataTable();

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
                            System.Data.DataTable tableSchema = await RetrieveAllDataAsync(szDatabaseMasterId, table);
                           


                            String data = "";
                            foreach (DataRow item in tableSchema.Rows)
                            {
                                data = data + '!' + item["Name"].ToString() + "~" + item["Id"].ToString();
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
                            System.Data.DataTable tableSchema= await RetrieveAllDataAsync(szDatabaseMasterId, referencedtable);                            

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
                                data = data + '!' + item[namecol].ToString() + "~" + item["Id"].ToString();
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
        void DeleteRowByName(System.Data.DataTable table, string name)
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
        public async Task<System.Data.DataTable> RetrieveAllTableReferencesAsync(int szDatabaseMasterId, string sourceTable, string targetTable)
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

        public async Task<bool> UpdateDataAsync(int szDatabaseMasterId,int rowid, string TableName, Dictionary<string, string> Data)
        {
            try
            {
                var sqlConnectionString = await _dbContext.DatabaseMasters.Where(i => i.Id == szDatabaseMasterId).FirstAsync();
                 return await _dataService.UpdateDataAsync(sqlConnectionString.ConnectionString,rowid, TableName,Data);
            }catch(Exception ex)
            {
                throw ex;
            }
            
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
