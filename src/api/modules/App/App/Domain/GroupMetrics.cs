using System.Diagnostics.Metrics;
using FSH.Starter.Aspire.ServiceDefaults;

namespace FSH.Starter.WebApi.App.Domain;

public static class GroupMetrics
{
    private static readonly Meter Meter = new Meter(MetricsConstants.App, "1.0.0");
    public static readonly Counter<int> Created = Meter.CreateCounter<int>("group.created");
    public static readonly Counter<int> Updated = Meter.CreateCounter<int>("group.updated");
    public static readonly Counter<int> Deleted = Meter.CreateCounter<int>("group.deleted");
}
