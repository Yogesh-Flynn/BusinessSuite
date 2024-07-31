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


//        <Project Sdk = "Microsoft.NET.Sdk.Web" >

//  < PropertyGroup >
//    < TargetFramework > net8.0</TargetFramework>
//    <Nullable>enable</Nullable>
//    <ImplicitUsings>enable</ImplicitUsings>
//    <UserSecretsId>aspnet-BusinessSuite-fbaed75c-8686-4408-b620-3ae58d9c317e</UserSecretsId>
//  </PropertyGroup>

//  <ItemGroup>
//    <PackageReference Include = "ClosedXML" Version="0.102.3" />
//    <PackageReference Include = "Hangfire" Version="1.8.14" />
//    <PackageReference Include = "Hangfire.AspNetCore" Version="1.8.14" />
//    <PackageReference Include = "Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Identity.UI" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.Common" Version="4.10.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.CSharp" Version="4.10.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.CSharp.Scripting" Version="4.10.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.Scripting.Common" Version="4.10.0" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version="8.0.7">
//      <PrivateAssets>all</PrivateAssets>
//      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
//    </PackageReference>
//    <PackageReference Include = "Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.3" />
//    <PackageReference Include = "X.PagedList" Version="10.0.3" />
//    <PackageReference Include = "X.PagedList.Mvc.Core" Version="10.0.3" />
//  </ItemGroup>

//</Project>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///

//        <Project Sdk = "Microsoft.NET.Sdk.Web" >

//  < PropertyGroup >
//    < TargetFramework > net8.0</TargetFramework>
//    <Nullable>enable</Nullable>
//    <ImplicitUsings>enable</ImplicitUsings>
//    <UserSecretsId>aspnet-BusinessSuite-fbaed75c-8686-4408-b620-3ae58d9c317e</UserSecretsId>
//  </PropertyGroup>

//  <ItemGroup>
//    <PackageReference Include = "ClosedXML" Version="0.102.3" />
//    <PackageReference Include = "Hangfire" Version="1.8.14" />
//    <PackageReference Include = "Hangfire.AspNetCore" Version="1.8.14" />
//    <PackageReference Include = "Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Identity.UI" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.Common" Version="4.8.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.CSharp" Version="4.8.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.CSharp.Scripting" Version="4.8.0" />
//    <PackageReference Include = "Microsoft.CodeAnalysis.Scripting.Common" Version="4.8.0" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.7" />
//    <PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version="8.0.7">
//      <PrivateAssets>all</PrivateAssets>
//      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
//    </PackageReference>
//    <PackageReference Include = "Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.3" />
//    <PackageReference Include = "X.PagedList" Version="10.0.3" />
//    <PackageReference Include = "X.PagedList.Mvc.Core" Version="10.0.3" />
//  </ItemGroup>

//</Project>

//PM> add-migration xxxxxx -c ApplicationDbContext
//Build started...
//Build succeeded.
//To undo this action, use Remove-Migration.
//PM> update-database -context ApplicationDbContext