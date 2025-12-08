namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceClaims;

public class InsuranceClaimViewModel
{
    public Guid PolicyId { get; set; }
    public string? ClaimType { get; set; }
    public decimal ClaimAmount { get; set; }
    public DateTimeOffset IncidentDate { get; set; } = DateTimeOffset.Now;
    public string? Description { get; set; }
    public string? SupportingDocuments { get; set; }
}
