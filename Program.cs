
using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using LongRunningAPIWithQueue.Services;
using LongRunningAPIWithQueue.Tasks;

namespace LongRunningAPIWithQueue;

/// <summary>
/// The Program Class
/// </summary>
public class Program
{
    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "LongRunningAPIWithQueue",
                Description = "An ASP.NET Core Web API with Queued Tasks",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "L. Maddalena",
                    Email = "l.maddalena@example.com",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        // IBackgroundTaskQueue
        builder.Services.AddSingleton<IBackgroundTaskQueue>(_ => 
        {
            if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
            {
                queueCapacity = 100;
            }

            return new BackgroundTaskQueue(queueCapacity);
        });

        // Tasks
        builder.Services.AddTransient<IWeatherForecastTask, WeatherForecastTask>();

        // QueuedHostedService
        builder.Services.AddHostedService<QueuedHostedService>();

        // add Serilog logging provider
        var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

        //builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
