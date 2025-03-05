using System.Diagnostics.Metrics;

namespace Accounting.Domain;
public static class AccountMetrics
{
    private static readonly Meter Meter = new("FSH.Accounting");
    public static readonly Counter<int> Created = Meter.CreateCounter<int>("accounting.accounts.created");
}
