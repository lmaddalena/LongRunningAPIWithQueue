namespace LongRunningAPIWithQueue.Tasks;

public interface IWeatherForecastTask
{
    ValueTask BuildWorkItemAsync(CancellationToken token);    
}

