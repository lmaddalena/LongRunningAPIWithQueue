namespace LongRunningAPIWithQueue;

/// <summary>
/// WeatherForecast class
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Date
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in C
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in F
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Summary
    /// </summary>
    public string? Summary { get; set; }
}
