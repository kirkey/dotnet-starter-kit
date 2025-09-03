namespace FSH.Starter.Blazor.Infrastructure.Metering;
public interface IUsageMeterService
{
    void Increment(string metric, double value = 1);
    IReadOnlyDictionary<string,double> Snapshot();
}
public class UsageMeterService : IUsageMeterService
{
    private readonly Dictionary<string,double> _metrics = new();
    private readonly object _lock = new();
    public void Increment(string metric, double value = 1)
    {
        lock(_lock){ _metrics.TryGetValue(metric, out var v); _metrics[metric]= v+value; }
    }
    public IReadOnlyDictionary<string,double> Snapshot()
    { lock(_lock) return _metrics.ToDictionary(k=>k.Key,v=>v.Value); }
}

