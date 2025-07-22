
using Amazon.S3;
using Amazon.SQS;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Infrastructure.Services;
using DevSkill.Inventory.Worker;
using Microsoft.EntityFrameworkCore;
using Serilog;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
var migrationAssemblyName = typeof(ImageResizeWorker).Assembly.FullName;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting...");
    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddHostedService<ImageResizeWorker>();
            var awsOptions = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSQS>();
            services.AddSingleton<ISqsService, SqsService>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start up failed");
}
finally
{
    Log.CloseAndFlush();
}