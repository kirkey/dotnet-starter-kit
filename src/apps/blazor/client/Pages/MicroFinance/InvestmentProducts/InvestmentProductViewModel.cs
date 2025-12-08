using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentProducts;

/// <summary>
/// ViewModel for creating/editing investment products.
/// </summary>
public class InvestmentProductViewModel
{
    public DefaultIdType Id { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [MaxLength(50)]
    public string? Code { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Product type is required")]
    public string? ProductType { get; set; }

    public string? Description { get; set; }

    public decimal MinInvestment { get; set; }

    public decimal MaxInvestment { get; set; }

    public decimal ExpectedReturn { get; set; }

    public string? RiskLevel { get; set; }

    public int MinTermMonths { get; set; }

    public int MaxTermMonths { get; set; }

    public string? Currency { get; set; }

    public bool IsCompounding { get; set; }

    public string? Status { get; set; }
}
