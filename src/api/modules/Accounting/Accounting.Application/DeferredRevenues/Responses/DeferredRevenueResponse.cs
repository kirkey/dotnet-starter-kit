namespace Accounting.Application.DeferredRevenues.Responses;

/// <summary>
/// Response model for deferred revenue entity.
/// </summary>
public sealed class DeferredRevenueResponse
{
    public DefaultIdType Id { get; set; }
    public string DeferredRevenueNumber { get; set; } = string.Empty;
    public DateTime RecognitionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public bool IsRecognized { get; set; }
    public DateTime? RecognizedDate { get; set; }
}

