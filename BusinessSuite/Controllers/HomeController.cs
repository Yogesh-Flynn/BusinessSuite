using BusinessSuite.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Diagnostics;
using System.Text;

namespace BusinessSuite.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
        //string code = "for (int i = 1; i <= 100; i++) { Console.WriteLine(i); }";

        //try
        //{
        //    // Redirect the console output to capture it
        //    var stringBuilder = new StringBuilder();
        //    using (var writer = new StringWriter(stringBuilder))
        //    {
        //        Console.SetOut(writer);

        //        // Collect non-dynamic assemblies for script options
        //        var references = AppDomain.CurrentDomain.GetAssemblies()
        //            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
        //            .Select(a => MetadataReference.CreateFromFile(a.Location))
        //            .Cast<MetadataReference>();

        //        var scriptOptions = ScriptOptions.Default
        //            .WithReferences(references)
        //            .WithImports("System", "System.IO", "System.Text");

        //        await CSharpScript.RunAsync(code, scriptOptions);
        //    }

        //    var result = stringBuilder.ToString();
        //    return Ok(result);
        //}
        //catch (CompilationErrorException ex)
        //{
        //    return BadRequest(string.Join(Environment.NewLine, ex.Diagnostics));
        //}
        //catch (Exception ex)
        //{
        //    return BadRequest(ex.Message);
        //}
        //finally
        //{
        //    // Reset the console output to its default
        //    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
