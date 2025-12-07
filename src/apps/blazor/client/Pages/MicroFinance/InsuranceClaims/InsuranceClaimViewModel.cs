namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceClaims;

/// <summary>
/// ViewModel for submitting insurance claims.
/// Maps to SubmitInsuranceClaimCommand.
/// </summary>
public class InsuranceClaimViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType PolicyId { get; set; }
    public string ClaimType { get; set; } = "Death";
    public decimal ClaimAmount { get; set; }
    public DateOnly IncidentDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string? Description { get; set; }
    public string? SupportingDocuments { get; set; }
}
