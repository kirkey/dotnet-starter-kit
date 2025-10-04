using System.Diagnostics.Metrics;
using FSH.Starter.Aspire.ServiceDefaults;

namespace FSH.Starter.WebApi.Todo.Domain;

/// <summary>
/// Provides metrics tracking for Todo module operations.
/// Tracks creation, updates, and deletions of todo items.
/// </summary>
public static class TodoMetrics
{
    /// <summary>
    /// Meter instance for tracking Todo-related metrics.
    /// </summary>
    private static readonly Meter Meter = new(MetricsConstants.Todos, "1.0.0");
    
    /// <summary>
    /// Counter for tracking the number of todo items created.
    /// </summary>
    public static readonly Counter<int> Created = Meter.CreateCounter<int>("todos.items.created", "items", "Number of todo items created");
    
    /// <summary>
    /// Counter for tracking the number of todo items updated.
    /// </summary>
    public static readonly Counter<int> Updated = Meter.CreateCounter<int>("todos.items.updated", "items", "Number of todo items updated");
    
    /// <summary>
    /// Counter for tracking the number of todo items deleted.
    /// </summary>
    public static readonly Counter<int> Deleted = Meter.CreateCounter<int>("todos.items.deleted", "items", "Number of todo items deleted");
}
