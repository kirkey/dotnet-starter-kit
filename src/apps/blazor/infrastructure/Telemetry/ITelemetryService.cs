namespace FSH.Starter.Blazor.Infrastructure.Telemetry;
public interface ITelemetryService
{
    void TrackEvent(string name, Dictionary<string,string>? props = null);
    void TrackError(Exception ex, Dictionary<string,string>? props = null);
    IReadOnlyList<TelemetryEvent> DrainBuffer();
}
public record TelemetryEvent(DateTime Utc, string Type, string Name, Dictionary<string,string>? Properties, string? Exception);
public class TelemetryService : ITelemetryService
{
    private readonly List<TelemetryEvent> _buffer = new();
    private readonly object _lock = new();
    public void TrackEvent(string name, Dictionary<string,string>? props = null)
    {
        lock(_lock) _buffer.Add(new TelemetryEvent(DateTime.UtcNow, "event", name, props, null));
    }
    public void TrackError(Exception ex, Dictionary<string,string>? props = null)
    {
        lock(_lock) _buffer.Add(new TelemetryEvent(DateTime.UtcNow, "error", ex.GetType().Name, props, ex.ToString()));
    }
    public IReadOnlyList<TelemetryEvent> DrainBuffer()
    {
        lock(_lock){ var c=_buffer.ToList(); _buffer.Clear(); return c; }
    }
}

