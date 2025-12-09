using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a cash vault, safe, or teller drawer at a branch for physical cash management.
/// Tracks cash balances, denominations, custody, and supports vault reconciliation.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Define branch cash storage locations (main vault, teller drawers, ATMs)</description></item>
///   <item><description>Track current cash balance and denomination breakdown</description></item>
///   <item><description>Set cash holding limits for security and insurance compliance</description></item>
///   <item><description>Record cash transfers between vaults (vault to drawer, branch to branch)</description></item>
///   <item><description>Support daily vault reconciliation and audit processes</description></item>
///   <item><description>Manage vault custodians and access controls</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Cash management is critical for MFIs that handle significant physical currency daily.
/// Proper vault management ensures:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Security</strong>: Cash limits reduce robbery risk and insurance costs</description></item>
///   <item><description><strong>Liquidity</strong>: Sufficient cash for expected disbursements and withdrawals</description></item>
///   <item><description><strong>Accountability</strong>: Clear custody chain for all cash movements</description></item>
///   <item><description><strong>Compliance</strong>: Audit trails for regulatory and insurance requirements</description></item>
/// </list>
/// <para><strong>Vault Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>MainVault</strong>: Primary branch safe with highest limits</description></item>
///   <item><description><strong>TellerDrawer</strong>: Individual teller cash drawer</description></item>
///   <item><description><strong>ATM</strong>: ATM cash cassettes</description></item>
///   <item><description><strong>MobileAgent</strong>: Field agent mobile money float</description></item>
///   <item><description><strong>Reserve</strong>: Emergency reserve cash</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Branch"/> - Branch this vault belongs to</description></item>
///   <item><description><see cref="TellerSession"/> - Sessions using this vault</description></item>
///   <item><description><see cref="Staff"/> - Vault custodian</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Creating a teller drawer vault</strong></para>
/// <code>
/// POST /api/microfinance/cash-vaults
/// {
///   "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "code": "VAULT-KGL-TD01",
///   "name": "Teller Drawer 01",
///   "vaultType": "TellerDrawer",
///   "location": "Counter 1, Main Hall",
///   "custodianStaffId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "minimumBalance": 100000,
///   "maximumBalance": 2000000,
///   "currentBalance": 500000
/// }
/// </code>
/// </example>
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
