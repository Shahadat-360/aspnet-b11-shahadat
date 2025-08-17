using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web;
using DevSkill.Inventory.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

#region Bootstrap Serilog
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Infrastructure.Seeds;
using DevSkill.Inventory.Web.Middlewares;
using Amazon.S3;
using Amazon.SQS;
using DevSkill.Inventory.Worker;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

#endregion

try
{
    Log.Information("Application Starting Up...");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly();

    #region Autoface Configuration 
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new WebModule(connectionString,migrationAssembly?.FullName));
    });
    #endregion

    #region Serilog Configuration 
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

    #endregion

    #region MediatR Configuration
    var mediatRAssembly = Assembly.Load("DevSkill.Inventory.Application");
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(mediatRAssembly);
    });
    #endregion

    #region WebHost Configuration
    //builder.WebHost.UseUrls("http://*:80");
    #endregion

    #region AutoMapper Configuration
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    #endregion

    #region Identity Configuration
    builder.Services.AddIdentity();
    #endregion

    #region Policy Configuration
    builder.Services.AddPolicy();
    #endregion

    #region Cookie Authentication Configuration
    builder.Services.AddCookieAuthentication();
    #endregion

    #region AWS Configuration
    var awsOptions = builder.Configuration.GetAWSOptions();
    builder.Services.AddDefaultAWSOptions(awsOptions);
    builder.Services.AddAWSService<IAmazonS3>();
    builder.Services.AddAWSService<IAmazonSQS>();
    #endregion

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,x=>x.MigrationsAssembly(migrationAssembly)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddScoped<AdminSeed>();
    builder.Services.AddHostedService<ImageResizeWorker>();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    var app = builder.Build();

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

    #region Admin Seed
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<AdminSeed>();
        await seeder.SeedAdminAsync();
    }
    #endregion

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();

    app.UseUserStatusCheck();

    app.UseAuthorization();

    app.MapStaticAssets();

    #region Area route configuration

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    #endregion

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}",
        defaults: new { area = "Admin", controller = "Dashboard", action = "Index" })
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "account",
        pattern: "Account/{action=Index}/{id?}",
        defaults: new { controller = "Account" })
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
