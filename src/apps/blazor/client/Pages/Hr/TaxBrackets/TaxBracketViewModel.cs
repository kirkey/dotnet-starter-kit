namespace FSH.Starter.Blazor.Client.Pages.Hr.TaxBrackets;

/// <summary>
/// ViewModel for TaxBracket CRUD operations.
/// Represents tax brackets for different income ranges and filing statuses.
/// </summary>
public class TaxBracketViewModel
{
    public DefaultIdType Id { get; set; }
    public string? TaxType { get; set; } = "Federal";
    public int Year { get; set; } = DateTime.Today.Year;
    public decimal MinIncome { get; set; }
    public decimal MaxIncome { get; set; }
    public decimal Rate { get; set; }
    public string? FilingStatus { get; set; } = "Single";
    public string? Description { get; set; }
}
