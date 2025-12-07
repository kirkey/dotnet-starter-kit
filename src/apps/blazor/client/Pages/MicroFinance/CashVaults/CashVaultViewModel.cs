using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CashVaults;

public class CashVaultViewModel
{
    public DefaultIdType Id { get; set; }

    [Required]
    public DefaultIdType BranchId { get; set; }

    [Required]
    [MinLength(3)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string VaultType { get; set; } = "MainVault";

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MinimumBalance { get; set; } = 10000;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MaximumBalance { get; set; } = 1000000;

    [Range(0, double.MaxValue)]
    public decimal OpeningBalance { get; set; }

    public string? Location { get; set; }

    public string? CustodianName { get; set; }

    public DefaultIdType? CustodianUserId { get; set; }

    // Read-only properties
    public decimal CurrentBalance { get; set; }
    public string? Status { get; set; }
    public DateTime? LastOpenedAt { get; set; }
    public DateTime? LastReconciliationAt { get; set; }
    public string? BranchName { get; set; }
}
