using BusinessSuite.Models;
using BusinessSuite.Models.Master_Models;
using Microsoft.AspNetCore.Identity;

namespace BusinessSuite.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var powerUser = new ApplicationUser
            {
                UserName = "admin@domain.com",
                Email = "admin@domain.com",
                FirstName = "SuperAdmin",
                LastName = "Test", 
            };

            string userPassword = "Admin@123";
            var user = await userManager.FindByEmailAsync("admin@domain.com");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }

            //////////////////////////////////////////////////////
            ///

            var websiteid = 0;
            if (!context.Websites.Any())
            {
                var website = new Website
                {
                    Name = "CRM",
                    Url = "",
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    Description="SKAUTECH CRM",
                    
                };
                var res= await context.Websites.AddAsync(website);

                int result=context.SaveChanges();
                websiteid = website.Id;
            }
            var dbmasterid = 0;
            if (!context.DatabaseMasters.Any())
            {
                var databaseMasters = new DatabaseMaster
                {
                    Name = "CRM",
                    CreatedDate = DateTime.Now,
                    ConnectionString = "Server=DESKTOP-NOQE41E;Database=CRM;Integrated Security=false;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;user=Development;password=Yogesh@964374",
                    IsDeleted = false,
                    WebsiteId=websiteid,

                };
                await context.DatabaseMasters.AddAsync(databaseMasters);

                context.SaveChanges();
                dbmasterid = databaseMasters.Id;
            }
            //if (!context.Modules.Any())
            //{
            //    var module = new Module
            //    {
            //        Name = "CRM",
            //        CreatedDate = DateTime.Now,
            //        IsDeleted = false,
            //        WebsiteId=websiteid,

            //    };
            //    await context.Modules.AddAsync(module);

            //    context.SaveChanges();
            //}
            if (!context.TableMasters.Any())
            {
                List<string> Tablenames = new List<string> { "Campaigns", "Customers" , "Marketings", "Products" };

                // Initialize the outer dictionary
                Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();

                // Method to add data
                void AddData(string outerKey, string innerKey, string value)
                {
                    // Check if the outer dictionary contains the outer key
                    if (!keyValuePairs.ContainsKey(outerKey))
                    {
                        // If not, create a new inner dictionary and add it to the outer dictionary
                        keyValuePairs[outerKey] = new Dictionary<string, string>();
                    }

                    // Add or update the inner dictionary with the new key-value pair
                    keyValuePairs[outerKey][innerKey] = value;
                }

                // Example usage
                AddData("Campaigns", "Id", "int");
                AddData("Campaigns", "CreatedDate", "DateTime");
                AddData("Campaigns", "Name", "nvarchar");
                AddData("Campaigns", "ScheduledDate", "DateTime");


                AddData("Customers", "Id", "int");
                AddData("Customers", "CreatedDate", "DateTime");
                AddData("Customers", "Name", "nvarchar");
                AddData("Customers", "PhoneNumber", "nvarchar");
                AddData("Customers", "Email", "nvarchar");
                AddData("Customers", "Domain", "nvarchar");
                AddData("Customers", "Address", "nvarchar");
                AddData("Customers", "BusinessName", "nvarchar");


                AddData("Marketings", "Id", "int");
                AddData("Marketings", "CreatedDate", "DateTime");
                AddData("Marketings", "Name", "nvarchar");
                AddData("Marketings", "Description", "nvarchar");
                AddData("Marketings", "Message", "nvarchar");
                AddData("Marketings", "ProductsId", "int");


                AddData("Products", "Id", "int");
                AddData("Products", "CreatedDate", "DateTime");
                AddData("Products", "Name", "nvarchar");
                AddData("Products", "Description", "nvarchar");
                AddData("Products", "UniqueCode", "nvarchar");
                AddData("Products", "Quantity", "int");



                foreach (var item in Tablenames)
                {

                    var tableMaster = new TableMaster
                    {
                        Name = item,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        DatabaseMasterId = dbmasterid,

                    };
                    await context.TableMasters.AddAsync(tableMaster);


                    context.SaveChanges();
                    var tableid = tableMaster.Id;
                    if (keyValuePairs.ContainsKey(item))
                    {
                        Console.WriteLine($"Inner pairs for {item}:");
                        foreach (var innerPair in keyValuePairs[item])
                        {
                            try
                            {
                                Console.WriteLine($"    Inner Key: {innerPair.Key}, Value: {innerPair.Value}");

                                var columnMaster = new ColumnMaster
                                {
                                    Name = innerPair.Key,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    DisplayName = innerPair.Key,
                                    Type = innerPair.Value,
                                    TableMasterId = tableid,
                                };
                                await context.ColumnMasters.AddAsync(columnMaster);

                                context.SaveChanges();
                            }catch(Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Outer pair name '{item}' not found.");
                    }
                  
                }

               
            }
                

        }
            
             
           
        
    }
}
