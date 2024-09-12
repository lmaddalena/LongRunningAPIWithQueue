using Microsoft.AspNetCore.Mvc;
using LongRunningAPIWithQueue.Services;
using LongRunningAPIWithQueue.Tasks;

namespace LongRunningAPIWithQueue.Controllers;

/// <summary>
/// WeatherForecastController class
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IWeatherForecastTask _weatherForecastTask;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBackgroundTaskQueue taskQueue, IWeatherForecastTask weatherForecastTask)
    {
        _logger = logger;
        _taskQueue = taskQueue;
        _weatherForecastTask = weatherForecastTask;
    }

    /// <summary>
    /// Get Weather forecast
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Enter GetWeatherForecast");

        _taskQueue.QueueBackgroundWorkItemAsync(_weatherForecastTask.BuildWorkItemAsync);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
