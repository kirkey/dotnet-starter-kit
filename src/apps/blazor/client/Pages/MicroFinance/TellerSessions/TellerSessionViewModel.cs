using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.TellerSessions;

public class TellerSessionViewModel
{
    public DefaultIdType Id { get; set; }

    [Required]
    public DefaultIdType BranchId { get; set; }

    [Required]
    public DefaultIdType CashVaultId { get; set; }

    [Required]
    public string SessionNumber { get; set; } = string.Empty;

    [Required]
    public DefaultIdType TellerUserId { get; set; }

    [Required]
    public string TellerName { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal OpeningBalance { get; set; }

    // Read-only properties
    public string? Status { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal TotalCashIn { get; set; }
    public decimal TotalCashOut { get; set; }
    public int TransactionCount { get; set; }
    public DateTime? OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime? PausedAt { get; set; }
    public string? BranchName { get; set; }
    public string? VaultName { get; set; }
    public decimal? ClosingBalance { get; set; }
    public decimal? Variance { get; set; }
}
