using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Branch and related entities.
/// </summary>
/// 
// Branch Events
public sealed record BranchCreated(Branch Branch) : DomainEvent;
public sealed record BranchUpdated(Branch Branch) : DomainEvent;
public sealed record BranchManagerAssigned(Guid BranchId, string ManagerName) : DomainEvent;
public sealed record BranchActivated(Guid BranchId) : DomainEvent;
public sealed record BranchDeactivated(Guid BranchId) : DomainEvent;
public sealed record BranchClosed(Guid BranchId, DateOnly ClosingDate) : DomainEvent;

// BranchTarget Events
public sealed record BranchTargetCreated(BranchTarget Target) : DomainEvent;
public sealed record BranchTargetUpdated(BranchTarget Target) : DomainEvent;
public sealed record BranchTargetProgressRecorded(
    Guid TargetId,
    decimal PreviousValue,
    decimal NewValue,
    decimal AchievementPercentage) : DomainEvent;
public sealed record BranchTargetAchieved(Guid TargetId, decimal AchievedValue) : DomainEvent;
public sealed record BranchTargetMissed(Guid TargetId, decimal AchievedValue, decimal TargetValue) : DomainEvent;

// CashVault Events
public sealed record CashVaultCreated(CashVault Vault) : DomainEvent;
public sealed record CashVaultUpdated(CashVault Vault) : DomainEvent;
public sealed record CashVaultDeposited(
    Guid VaultId,
    decimal Amount,
    decimal PreviousBalance,
    decimal NewBalance) : DomainEvent;
public sealed record CashVaultWithdrawn(
    Guid VaultId,
    decimal Amount,
    decimal PreviousBalance,
    decimal NewBalance) : DomainEvent;
public sealed record CashVaultDayOpened(Guid VaultId, decimal VerifiedBalance) : DomainEvent;
public sealed record CashVaultReconciled(
    Guid VaultId,
    decimal ReconciledBalance,
    DateTime ReconciliationTime) : DomainEvent;
public sealed record CashVaultDiscrepancyFound(
    Guid VaultId,
    decimal ExpectedBalance,
    decimal ActualBalance,
    decimal Difference) : DomainEvent;
public sealed record CashVaultBelowMinimum(Guid VaultId, decimal CurrentBalance, decimal MinimumBalance) : DomainEvent;
public sealed record CashVaultOverLimit(Guid VaultId, decimal CurrentBalance, decimal MaximumBalance) : DomainEvent;
public sealed record CashVaultLocked(Guid VaultId) : DomainEvent;
public sealed record CashVaultUnlocked(Guid VaultId) : DomainEvent;

// TellerSession Events
public sealed record TellerSessionOpened(TellerSession Session) : DomainEvent;
public sealed record TellerSessionCashInRecorded(
    Guid SessionId,
    decimal Amount,
    decimal ExpectedBalance) : DomainEvent;
public sealed record TellerSessionCashOutRecorded(
    Guid SessionId,
    decimal Amount,
    decimal ExpectedBalance) : DomainEvent;
public sealed record TellerSessionPaused(Guid SessionId) : DomainEvent;
public sealed record TellerSessionResumed(Guid SessionId) : DomainEvent;
public sealed record TellerSessionClosed(
    Guid SessionId,
    decimal ExpectedBalance,
    decimal ActualBalance,
    decimal Variance) : DomainEvent;
public sealed record TellerSessionVerified(
    Guid SessionId,
    Guid SupervisorUserId,
    string SupervisorName) : DomainEvent;

public sealed record TellerSessionCashTransferred(
    Guid SessionId,
    decimal Amount,
    bool IsTransferIn,
    string Reference,
    decimal ExpectedBalance) : DomainEvent;

public sealed record CashVaultDayClosed(Guid VaultId, decimal VerifiedBalance) : DomainEvent;

public sealed record CashVaultTransferred(
    Guid SourceVaultId,
    Guid TargetVaultId,
    decimal Amount,
    decimal PreviousBalance,
    decimal NewBalance) : DomainEvent;
