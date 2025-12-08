using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanDisbursementTranches;

/// <summary>
/// ViewModel for creating/editing loan disbursement tranches.
/// </summary>
public class LoanDisbursementTrancheViewModel
{
    public DefaultIdType Id { get; set; }

    [Required(ErrorMessage = "Loan ID is required")]
    public DefaultIdType LoanId { get; set; }

    [Required(ErrorMessage = "Tranche number is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Tranche number must be at least 1")]
    public int TrancheNumber { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    public DateTime? ScheduledDate { get; set; }

    public DateTime? DisbursementDate { get; set; }

    public string? DisbursementMethod { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? Reference { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
}
