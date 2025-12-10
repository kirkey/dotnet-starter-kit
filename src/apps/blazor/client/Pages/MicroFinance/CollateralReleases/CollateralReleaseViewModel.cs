namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralReleases;

public class CollateralReleaseViewModel
{
    public DefaultIdType CollateralId { get; set; }
    public DefaultIdType LoanId { get; set; }
    public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;
    public string ReleaseMethod { get; set; } = string.Empty;
    public string? RecipientName { get; set; }
    public string? RecipientIdNumber { get; set; }
    public string? RecipientContact { get; set; }
    public string? Notes { get; set; }
}
