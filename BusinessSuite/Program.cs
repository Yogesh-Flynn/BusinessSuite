using BusinessSuite.Data;
using BusinessSuite.Interfaces;
using BusinessSuite.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Hangfire;
using Hangfire.MySql;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDbContext<CRMDbContext>(options =>
//          options.UseSqlServer(builder.Configuration.GetConnectionString("CRMDBCONN")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)))); // Adjust MySQL version as needed

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<CRMDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("CRMDBCONN"),
    new MySqlServerVersion(new Version(8, 0, 33)))); // Adjust MySQL version as needed



builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddScoped<ICatalogueService, CatalogueService>();


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Configure Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(new MySqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlStorageOptions
    {
        TablesPrefix = "Hangfire", // Optional: Use a table prefix if needed
        QueuePollInterval = TimeSpan.FromSeconds(15), // Set your preferred poll interval
        TransactionIsolationLevel = (System.Transactions.IsolationLevel?)System.Data.IsolationLevel.ReadCommitted, // Set transaction isolation level
        JobExpirationCheckInterval = TimeSpan.FromHours(1), // Set the interval for expired job checks
        CountersAggregateInterval = TimeSpan.FromMinutes(5), // Aggregation interval for counters
        PrepareSchemaIfNecessary = true, // Automatically create the schema if necessary
        DashboardJobListLimit = 5000, // Job list limit for the Hangfire Dashboard
    })));

// Add Hangfire server
builder.Services.AddHangfireServer();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Add application services.
builder.Services.AddScoped<MyJobService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await ApplicationDbInitializer.SeedRolesAndUsersAsync(services, context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// Use Hangfire Dashboard
app.UseHangfireDashboard();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
