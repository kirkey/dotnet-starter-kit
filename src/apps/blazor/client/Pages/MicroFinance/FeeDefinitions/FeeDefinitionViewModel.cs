namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeDefinitions;

/// <summary>
/// ViewModel for creating and updating fee definition entities.
/// Maps form data to <see cref="CreateFeeDefinitionCommand"/>.
/// </summary>
public class FeeDefinitionViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique code for the fee.
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// Name of the fee.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Description of the fee.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Type of fee (e.g., ServiceFee, ProcessingFee, PenaltyFee).
    /// </summary>
    public string? FeeType { get; set; }
    
    /// <summary>
    /// How the fee is calculated (Flat, Percentage, Tiered).
    /// </summary>
    public string? CalculationType { get; set; }
    
    /// <summary>
    /// What this fee applies to (Loan, Savings, Share, All).
    /// </summary>
    public string? AppliesTo { get; set; }
    
    /// <summary>
    /// How often the fee is charged (OneTime, Monthly, Annually).
    /// </summary>
    public string? ChargeFrequency { get; set; }
    
    /// <summary>
    /// Fee amount (flat amount or percentage).
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Minimum amount for percentage calculations.
    /// </summary>
    public decimal? MinAmount { get; set; }
    
    /// <summary>
    /// Maximum amount for percentage calculations.
    /// </summary>
    public decimal? MaxAmount { get; set; }
    
    /// <summary>
    /// Whether the fee is taxable.
    /// </summary>
    public bool IsTaxable { get; set; }
    
    /// <summary>
    /// Tax rate if taxable.
    /// </summary>
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// Whether this fee is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
