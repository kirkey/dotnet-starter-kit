namespace FSH.Starter.Blazor.Client.Pages.Accounting.DeferredRevenue;

/// <summary>
/// ViewModel used for creating or editing deferred revenue entries.
/// </summary>
public sealed class DeferredRevenueViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? DeferredRevenueNumber { get; set; }
    public DateTime? RecognitionDate { get; set; } = DateTime.Today.AddMonths(1);
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public bool IsRecognized { get; set; }
    public DateTime? RecognizedDate { get; set; }
}

