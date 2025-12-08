namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskCategories;

public class RiskCategoryViewModel
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? RiskType { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? DefaultSeverity { get; set; }
    public decimal WeightFactor { get; set; } = 1.0m;
    public decimal? AlertThreshold { get; set; }
    public bool RequiresEscalation { get; set; }
    public int? EscalationHours { get; set; }
    public int DisplayOrder { get; set; }
    public string? Status { get; set; }
}
