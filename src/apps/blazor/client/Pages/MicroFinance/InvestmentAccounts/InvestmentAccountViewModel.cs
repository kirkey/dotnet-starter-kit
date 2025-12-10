using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentAccounts;

/// <summary>
/// ViewModel for creating/editing investment accounts.
/// </summary>
public class InvestmentAccountViewModel
{
    public DefaultIdType Id { get; set; }

    [Required(ErrorMessage = "Account number is required")]
    [MaxLength(50)]
    public string? AccountNumber { get; set; }

    [Required(ErrorMessage = "Customer ID is required")]
    public DefaultIdType CustomerId { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public DefaultIdType ProductId { get; set; }

    [Required(ErrorMessage = "Invested amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Invested amount must be greater than 0")]
    public decimal InvestedAmount { get; set; }

    public decimal CurrentValue { get; set; }

    public DateTime? OpeningDate { get; set; }

    public DateTime? MaturityDate { get; set; }

    public decimal InterestRate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public DateTime? FirstInvestmentDate { get; set; }

    public DateTime? LastTransactionDate { get; set; }

    public DateTime? NextSipDate { get; set; }

    public DateTime? TargetDate { get; set; }
}
