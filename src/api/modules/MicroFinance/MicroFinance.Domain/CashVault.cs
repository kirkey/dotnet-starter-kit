using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a cash vault, safe, or teller drawer at a branch for physical cash management.
/// Tracks cash balances, denominations, custody, and supports vault reconciliation.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define branch cash storage locations (main vault, teller drawers, ATMs).
/// - Track current cash balance and denomination breakdown.
/// - Set cash holding limits for security and insurance compliance.
/// - Record cash transfers between vaults (vault to drawer, branch to branch).
/// - Support daily vault reconciliation and audit processes.
/// - Manage vault custodians and access controls.
/// 
/// Default values and constraints:
/// - Code: required unique identifier, max 32 characters (example: "VAULT-KGL-TD01")
/// - VaultType: required, one of MainVault, TellerDrawer, ATM, MobileAgent, Reserve
/// - BranchId: required, must reference the branch this vault belongs to
/// - Location: optional description of physical location, max 256 characters
/// - CustodianStaffId: optional, references staff member responsible for vault
/// - MinimumBalance: minimum cash holding threshold for alerts
/// - MaximumBalance: maximum cash holding limit for security
/// - CurrentBalance: 0 by default, updated by transactions
/// 
/// Business rules:
/// - Code must be unique within the system.
/// - CurrentBalance cannot exceed MaximumBalance.
/// - Cash transfers require dual authorization for amounts above threshold.
/// - Daily reconciliation required before shift end.
/// - Vault access logged for audit trail.
/// - Cannot close vault with non-zero balance.
/// - Insurance limits determine maximum cash holding allowed.
/// </remarks>
/// <seealso cref="Branch"/>
/// <seealso cref="TellerSession"/>
/// <seealso cref="Staff"/>
public sealed class CashVault : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int VaultType = 32;
        public const int Location = 256;
        public const int CustodianName = 128;
    }

    /// <summary>
    /// Vault type classification.
    /// </summary>
    public const string TypeMainVault = "MainVault";
    public const string TypeTellerDrawer = "TellerDrawer";
    public const string TypeATM = "ATM";
    public const string TypeMobileAgent = "MobileAgent";
    public const string TypeReserve = "Reserve";

    /// <summary>
    /// Operational status of the vault.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusLocked = "Locked";
    public const string StatusUnderAudit = "UnderAudit";

    /// <summary>
    /// Reference to the branch this vault belongs to.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Unique code for the vault.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Type of vault.
    /// </summary>
    public string VaultType { get; private set; } = TypeMainVault;

    /// <summary>
    /// Current cash balance in the vault.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Opening balance at the start of the business day.
    /// </summary>
    public decimal OpeningBalance { get; private set; }

    /// <summary>
    /// Minimum cash level before replenishment is needed.
    /// </summary>
    public decimal MinimumBalance { get; private set; }

    /// <summary>
    /// Maximum cash holding limit for security.
    /// </summary>
    public decimal MaximumBalance { get; private set; }

    /// <summary>
    /// Physical location within the branch.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Name of the custodian responsible for the vault.
    /// </summary>
    public string? CustodianName { get; private set; }

    /// <summary>
    /// User ID of the custodian.
    /// </summary>
    public Guid? CustodianUserId { get; private set; }

    /// <summary>
    /// Last reconciliation date.
    /// </summary>
    public DateTime? LastReconciliationDate { get; private set; }

    /// <summary>
    /// Last reconciled balance.
    /// </summary>
    public decimal? LastReconciledBalance { get; private set; }

    /// <summary>
    /// Denomination breakdown in JSON format.
    /// </summary>
    public string? DenominationBreakdown { get; private set; }

    /// <summary>
    /// Current status of the vault.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    // Navigation properties
    public Branch Branch { get; private set; } = null!;
    public ICollection<TellerSession> TellerSessions { get; private set; } = new List<TellerSession>();

    private CashVault() { }

    /// <summary>
    /// Creates a new cash vault.
    /// </summary>
    public static CashVault Create(
        Guid branchId,
        string code,
        string name,
        string vaultType,
        decimal minimumBalance,
        decimal maximumBalance,
        decimal openingBalance = 0,
        string? location = null,
        string? custodianName = null,
        Guid? custodianUserId = null)
    {
        var vault = new CashVault
        {
            BranchId = branchId,
            Code = code,
            VaultType = vaultType,
            MinimumBalance = minimumBalance,
            MaximumBalance = maximumBalance,
            OpeningBalance = openingBalance,
            CurrentBalance = openingBalance,
            Location = location,
            CustodianName = custodianName,
            CustodianUserId = custodianUserId,
            Status = StatusActive
        };
        vault.Name = name;

        vault.QueueDomainEvent(new CashVaultCreated(vault));
        return vault;
    }

    /// <summary>
    /// Updates vault configuration.
    /// </summary>
    public CashVault Update(
        string? name,
        decimal? minimumBalance,
        decimal? maximumBalance,
        string? location,
        string? custodianName,
        Guid? custodianUserId,
        string? notes)
    {
        if (name is not null) Name = name;
        if (minimumBalance.HasValue) MinimumBalance = minimumBalance.Value;
        if (maximumBalance.HasValue) MaximumBalance = maximumBalance.Value;
        if (location is not null) Location = location;
        if (custodianName is not null) CustodianName = custodianName;
        if (custodianUserId.HasValue) CustodianUserId = custodianUserId.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new CashVaultUpdated(this));
        return this;
    }

    /// <summary>
    /// Deposits cash into the vault.
    /// </summary>
    public void Deposit(decimal amount, string? denominationBreakdown = null)
    {
        if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        var previousBalance = CurrentBalance;
        CurrentBalance += amount;

        if (denominationBreakdown is not null)
        {
            DenominationBreakdown = denominationBreakdown;
        }

        QueueDomainEvent(new CashVaultDeposited(Id, amount, previousBalance, CurrentBalance));

        // Check if over maximum limit
        if (CurrentBalance > MaximumBalance)
        {
            QueueDomainEvent(new CashVaultOverLimit(Id, CurrentBalance, MaximumBalance));
        }
    }

    /// <summary>
    /// Withdraws cash from the vault.
    /// </summary>
    public void Withdraw(decimal amount, string? denominationBreakdown = null)
    {
        if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
        if (amount > CurrentBalance) throw new InvalidOperationException("Insufficient vault balance.");

        var previousBalance = CurrentBalance;
        CurrentBalance -= amount;

        if (denominationBreakdown is not null)
        {
            DenominationBreakdown = denominationBreakdown;
        }

        QueueDomainEvent(new CashVaultWithdrawn(Id, amount, previousBalance, CurrentBalance));

        // Check if below minimum threshold
        if (CurrentBalance < MinimumBalance)
        {
            QueueDomainEvent(new CashVaultBelowMinimum(Id, CurrentBalance, MinimumBalance));
        }
    }

    /// <summary>
    /// Opens the vault for a new business day.
    /// </summary>
    public void OpenDay(decimal verifiedBalance)
    {
        OpeningBalance = verifiedBalance;
        CurrentBalance = verifiedBalance;
        QueueDomainEvent(new CashVaultDayOpened(Id, verifiedBalance));
    }

    /// <summary>
    /// Performs end-of-day reconciliation.
    /// </summary>
    public void Reconcile(decimal physicalCount, string? denominationBreakdown = null)
    {
        var difference = physicalCount - CurrentBalance;

        LastReconciliationDate = DateTime.UtcNow;
        LastReconciledBalance = physicalCount;

        if (denominationBreakdown is not null)
        {
            DenominationBreakdown = denominationBreakdown;
        }

        if (difference != 0)
        {
            QueueDomainEvent(new CashVaultDiscrepancyFound(Id, CurrentBalance, physicalCount, difference));
        }

        CurrentBalance = physicalCount;
        QueueDomainEvent(new CashVaultReconciled(Id, physicalCount, LastReconciliationDate.Value));
    }

    /// <summary>
    /// Locks the vault for audit.
    /// </summary>
    public void Lock()
    {
        Status = StatusLocked;
        QueueDomainEvent(new CashVaultLocked(Id));
    }

    /// <summary>
    /// Unlocks the vault.
    /// </summary>
    public void Unlock()
    {
        Status = StatusActive;
        QueueDomainEvent(new CashVaultUnlocked(Id));
    }

    /// <summary>
    /// Closes the vault for the business day.
    /// </summary>
    public void CloseDay(decimal verifiedBalance, string? denominationBreakdown = null)
    {
        var difference = verifiedBalance - CurrentBalance;
        
        if (denominationBreakdown is not null)
        {
            DenominationBreakdown = denominationBreakdown;
        }

        LastReconciliationDate = DateTime.UtcNow;
        LastReconciledBalance = verifiedBalance;
        CurrentBalance = verifiedBalance;

        if (difference != 0)
        {
            QueueDomainEvent(new CashVaultDiscrepancyFound(Id, CurrentBalance, verifiedBalance, difference));
        }

        QueueDomainEvent(new CashVaultDayClosed(Id, verifiedBalance));
    }

    /// <summary>
    /// Transfers cash to another vault.
    /// </summary>
    public void TransferTo(CashVault targetVault, decimal amount, string? denominationBreakdown = null)
    {
        if (amount <= 0) throw new ArgumentException("Transfer amount must be positive.", nameof(amount));
        if (amount > CurrentBalance) throw new InvalidOperationException("Insufficient vault balance for transfer.");
        if (targetVault.Id == Id) throw new InvalidOperationException("Cannot transfer to the same vault.");

        // Withdraw from this vault
        var previousBalance = CurrentBalance;
        CurrentBalance -= amount;

        // Deposit to target vault
        targetVault.Deposit(amount, denominationBreakdown);

        QueueDomainEvent(new CashVaultTransferred(Id, targetVault.Id, amount, previousBalance, CurrentBalance));
    }
}
