using BusinessSuite.Data;
using BusinessSuite.Models.Master_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessSuite.Controllers
{
    public class WebsiteController : Controller
    {
        private ApplicationDbContext _ApplicationDbContext { get; set; }
        public WebsiteController(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
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
        public async Task<IActionResult> GetSidebarData()
        {
            var data =await _ApplicationDbContext.Websites.ToListAsync();

            return Json(data);
        }
    }
}
