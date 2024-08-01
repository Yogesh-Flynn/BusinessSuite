using BusinessSuite.Data;
using BusinessSuite.Models.Master_Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BusinessSuite.Controllers
{
    public class WebsiteController : Controller
    {
        private ApplicationDbContext _ApplicationDbContext { get; set; }
        private readonly IConfiguration _configuration;
        public WebsiteController(ApplicationDbContext applicationDbContext,IConfiguration configuration)
        {
            _ApplicationDbContext = applicationDbContext;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(int websiteid)
        {
            var websitedata= await _ApplicationDbContext.Websites.Where(i=>i.Id == websiteid).FirstOrDefaultAsync();
            MasterUICodes code =await _ApplicationDbContext.MasterUICodes.Where(i=>i.WebsiteId == websiteid).FirstOrDefaultAsync(); 
            string message =code.PageCode;
            ViewBag.HtmlContent = message;
            ViewBag.WebsiteId = websiteid;
            TempData["WebsiteId"] = websiteid;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddWebsite()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWebsite(Website website)
        {
            
                website.IsDeleted = false;
                var res = await _ApplicationDbContext.Websites.AddAsync(website);

                int result = _ApplicationDbContext.SaveChanges();
                int websiteid = website.Id;


                var databaseMasters = new DatabaseMaster
                {
                    Name = "CRM",
                    CreatedDate = DateTime.Now,
                    ConnectionString = $"Server=DESKTOP-NOQE41E;Database={website.Name};Integrated Security=false;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;user=Development;password=Yogesh@964374",
                    IsDeleted = false,
                    WebsiteId = websiteid,

                };
                await _ApplicationDbContext.DatabaseMasters.AddAsync(databaseMasters);

                _ApplicationDbContext.SaveChanges();
                int dbmasterid = databaseMasters.Id;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = $"IF DB_ID('{website.Name}') IS NULL CREATE DATABASE [{website.Name}]";
                    command.ExecuteNonQuery();

                    // Save the website data to the database or perform other actions
                    // For now, just return a success message
                    TempData["Message"] = "Website added successfully!";
                }
                
                return View(website);
        }
        public async Task<IActionResult> GetSidebarData()
        {
            var data =await _ApplicationDbContext.Websites.ToListAsync();

            return Json(data);
        }
    }
}
