namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CashVaults;

/// <summary>
/// ViewModel used by the CashVaults page for add/edit operations.
/// Mirrors the shape of the API's CreateCashVaultCommand and UpdateCashVaultCommand so Mapster/Adapt can map between them.
/// </summary>
public class CashVaultViewModel
{
    /// <summary>
    /// Primary identifier of the cash vault.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique vault code assigned by the system.
    /// Example: "VLT-001", "VAULT-10001".
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Name of the vault. Required.
    /// Example: "Main Vault", "Branch Vault A".
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// ID of the branch where the vault is located.
    /// </summary>
    public DefaultIdType BranchId { get; set; }

    /// <summary>
    /// Type of vault.
    /// Values: "MainVault", "BranchVault", "ATM", "SafeDeposit".
    /// </summary>
    public string? VaultType { get; set; }

    /// <summary>
    /// Current status of the vault.
    /// Values: "Active", "Closed", "Suspended".
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Current cash balance in the vault.
    /// </summary>
    public decimal CurrentBalance { get; set; }

    /// <summary>
    /// Opening balance when the vault was created.
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Minimum balance threshold for the vault.
    /// Alerts will be triggered if balance falls below this.
    /// </summary>
    public decimal MinimumBalance { get; set; }

    /// <summary>
    /// Maximum balance threshold for the vault.
    /// Excess should be transferred to main vault.
    /// </summary>
    public decimal MaximumBalance { get; set; }

    /// <summary>
    /// Physical location or room where the vault is situated.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Name of the custodian responsible for the vault.
    /// </summary>
    public string? CustodianName { get; set; }

    /// <summary>
    /// User ID of the custodian if linked to system user.
    /// </summary>
    public DefaultIdType? CustodianUserId { get; set; }

    /// <summary>
    /// Date of the last reconciliation.
    /// </summary>
    public DateTime? LastReconciliationDate { get; set; }

    /// <summary>
    /// Balance recorded at the last reconciliation.
    /// </summary>
    public decimal? LastReconciledBalance { get; set; }

    /// <summary>
    /// Additional notes about the vault.
    /// </summary>
    public string? Notes { get; set; }
}
