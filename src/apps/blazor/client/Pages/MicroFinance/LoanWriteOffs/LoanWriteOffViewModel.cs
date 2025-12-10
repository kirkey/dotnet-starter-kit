namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanWriteOffs;

/// <summary>
/// View model for loan write-off creation.
/// </summary>
public class LoanWriteOffViewModel
{
    public DefaultIdType LoanId { get; set; }
    public string WriteOffType { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;
    public decimal PrincipalWriteOff { get; set; }
    public decimal InterestWriteOff { get; set; }
    public decimal PenaltiesWriteOff { get; set; }
    public decimal FeesWriteOff { get; set; }
    public int DaysPastDue { get; set; }
    public int CollectionAttempts { get; set; }
}
