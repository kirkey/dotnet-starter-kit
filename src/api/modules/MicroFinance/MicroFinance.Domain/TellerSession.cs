using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a teller working session with assigned cash drawer.
/// Tracks teller transactions and supports session reconciliation for cash management.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Open and close daily teller working sessions</description></item>
///   <item><description>Track cash in/out transactions during the session</description></item>
///   <item><description>Reconcile physical cash with system records at session end</description></item>
///   <item><description>Record cash shortages and overages for accountability</description></item>
///   <item><description>Support supervisor approval for session opening/closing</description></item>
///   <item><description>Provide audit trail for all cash handling activities</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Teller sessions are critical for MFI cash management and internal controls:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Opening Balance</strong>: Cash counted and verified at session start</description></item>
///   <item><description><strong>Cash Receipts</strong>: Loan repayments, savings deposits, fee collections</description></item>
///   <item><description><strong>Cash Disbursements</strong>: Loan disbursements, savings withdrawals</description></item>
///   <item><description><strong>Closing Balance</strong>: Physical count must match calculated balance</description></item>
///   <item><description><strong>Reconciliation</strong>: Any variance is recorded as shortage/overage</description></item>
/// </list>
/// <para><strong>Session Status:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Open</strong>: Teller actively processing transactions</description></item>
///   <item><description><strong>Paused</strong>: Temporary break (lunch, bathroom)</description></item>
///   <item><description><strong>Closed</strong>: Session ended, awaiting reconciliation</description></item>
///   <item><description><strong>Reconciled</strong>: Cash balanced, session complete</description></item>
///   <item><description><strong>Shortage</strong>: Physical cash less than expected</description></item>
///   <item><description><strong>Overage</strong>: Physical cash more than expected</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Staff"/> - Teller staff member</description></item>
///   <item><description><see cref="CashVault"/> - Cash drawer assigned to teller</description></item>
///   <item><description><see cref="Branch"/> - Branch where session occurs</description></item>
///   <item><description><see cref="SavingsTransaction"/> - Savings transactions in session</description></item>
///   <item><description><see cref="LoanRepayment"/> - Loan payments received</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Opening a teller session</strong></para>
/// <code>
/// POST /api/microfinance/teller-sessions/open
/// {
///   "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "cashVaultId": "8cb96421-7c8e-4fd6-b9e2-1a2b3c4d5e6f",
///   "tellerId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "openingBalance": 500000,
///   "supervisorId": "b2c3d4e5-6f78-9012-bcde-f12345678901"
/// }
/// </code>
/// </example>
public sealed class TellerSession : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int SessionNumber = 32;
        public const int TellerName = 128;
        public const int SupervisorName = 128;
    }

    /// <summary>
    /// Session status values.
    /// </summary>
    public const string StatusOpen = "Open";
    public const string StatusPaused = "Paused";
    public const string StatusClosed = "Closed";
    public const string StatusReconciled = "Reconciled";
    public const string StatusShortage = "Shortage";
    public const string StatusOverage = "Overage";

    /// <summary>
    /// Reference to the branch.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Reference to the cash vault/drawer assigned.
    /// </summary>
    public Guid CashVaultId { get; private set; }

    /// <summary>
    /// Unique session number for the day.
    /// </summary>
    public string SessionNumber { get; private set; } = string.Empty;

    /// <summary>
    /// User ID of the teller.
    /// </summary>
    public Guid TellerUserId { get; private set; }

    /// <summary>
    /// Display name of the teller.
    /// </summary>
    public string TellerName { get; private set; } = string.Empty;

    /// <summary>
    /// Date of the session.
    /// </summary>
    public DateOnly SessionDate { get; private set; }

    /// <summary>
    /// Time when the session started.
    /// </summary>
    public DateTime StartTime { get; private set; }

    /// <summary>
    /// Time when the session ended.
    /// </summary>
    public DateTime? EndTime { get; private set; }

    /// <summary>
    /// Opening cash balance at session start.
    /// </summary>
    public decimal OpeningBalance { get; private set; }

    /// <summary>
    /// Total cash received during the session.
    /// </summary>
    public decimal TotalCashIn { get; private set; }

    /// <summary>
    /// Total cash paid out during the session.
    /// </summary>
    public decimal TotalCashOut { get; private set; }

    /// <summary>
    /// Expected closing balance (Opening + CashIn - CashOut).
    /// </summary>
    public decimal ExpectedClosingBalance { get; private set; }

    /// <summary>
    /// Actual physical cash count at session end.
    /// </summary>
    public decimal? ActualClosingBalance { get; private set; }

    /// <summary>
    /// Difference between expected and actual (for shortages/overages).
    /// </summary>
    public decimal? Variance { get; private set; }

    /// <summary>
    /// Number of cash transactions processed.
    /// </summary>
    public int TransactionCount { get; private set; }

    /// <summary>
    /// Current status of the session.
    /// </summary>
    public string Status { get; private set; } = StatusOpen;

    /// <summary>
    /// User ID of supervisor who verified the session.
    /// </summary>
    public Guid? SupervisorUserId { get; private set; }

    /// <summary>
    /// Name of the supervisor.
    /// </summary>
    public string? SupervisorName { get; private set; }

    /// <summary>
    /// Time when supervisor verified the session.
    /// </summary>
    public DateTime? SupervisorVerificationTime { get; private set; }

    /// <summary>
    /// Denomination breakdown at close in JSON format.
    /// </summary>
    public string? ClosingDenominations { get; private set; }

    // Navigation properties
    public Branch Branch { get; private set; } = null!;
    public CashVault CashVault { get; private set; } = null!;

    private TellerSession() { }

    /// <summary>
    /// Opens a new teller session.
    /// </summary>
    public static TellerSession Open(
        Guid branchId,
        Guid cashVaultId,
        string sessionNumber,
        Guid tellerUserId,
        string tellerName,
        decimal openingBalance)
    {
        var session = new TellerSession
        {
            BranchId = branchId,
            CashVaultId = cashVaultId,
            SessionNumber = sessionNumber,
            TellerUserId = tellerUserId,
            TellerName = tellerName,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = DateTime.UtcNow,
            OpeningBalance = openingBalance,
            TotalCashIn = 0,
            TotalCashOut = 0,
            ExpectedClosingBalance = openingBalance,
            TransactionCount = 0,
            Status = StatusOpen
        };

        session.QueueDomainEvent(new TellerSessionOpened(session));
        return session;
    }

    /// <summary>
    /// Records a cash-in transaction.
    /// </summary>
    public void RecordCashIn(decimal amount)
    {
        if (Status != StatusOpen && Status != StatusPaused)
            throw new InvalidOperationException("Cannot record transactions on a closed session.");

        TotalCashIn += amount;
        ExpectedClosingBalance = OpeningBalance + TotalCashIn - TotalCashOut;
        TransactionCount++;

        QueueDomainEvent(new TellerSessionCashInRecorded(Id, amount, ExpectedClosingBalance));
    }

    /// <summary>
    /// Records a cash-out transaction.
    /// </summary>
    public void RecordCashOut(decimal amount)
    {
        if (Status != StatusOpen && Status != StatusPaused)
            throw new InvalidOperationException("Cannot record transactions on a closed session.");

        if (amount > ExpectedClosingBalance)
            throw new InvalidOperationException("Insufficient cash in drawer for this transaction.");

        TotalCashOut += amount;
        ExpectedClosingBalance = OpeningBalance + TotalCashIn - TotalCashOut;
        TransactionCount++;

        QueueDomainEvent(new TellerSessionCashOutRecorded(Id, amount, ExpectedClosingBalance));
    }

    /// <summary>
    /// Pauses the session (e.g., for lunch break).
    /// </summary>
    public void Pause()
    {
        if (Status != StatusOpen)
            throw new InvalidOperationException("Only open sessions can be paused.");

        Status = StatusPaused;
        QueueDomainEvent(new TellerSessionPaused(Id));
    }

    /// <summary>
    /// Resumes a paused session.
    /// </summary>
    public void Resume()
    {
        if (Status != StatusPaused)
            throw new InvalidOperationException("Only paused sessions can be resumed.");

        Status = StatusOpen;
        QueueDomainEvent(new TellerSessionResumed(Id));
    }

    /// <summary>
    /// Closes the session with physical cash count.
    /// </summary>
    public void Close(decimal actualClosingBalance, string? closingDenominations = null)
    {
        if (Status == StatusClosed || Status == StatusReconciled)
            throw new InvalidOperationException("Session is already closed.");

        EndTime = DateTime.UtcNow;
        ActualClosingBalance = actualClosingBalance;
        ClosingDenominations = closingDenominations;
        Variance = actualClosingBalance - ExpectedClosingBalance;

        if (Variance == 0)
        {
            Status = StatusReconciled;
        }
        else if (Variance < 0)
        {
            Status = StatusShortage;
        }
        else
        {
            Status = StatusOverage;
        }

        QueueDomainEvent(new TellerSessionClosed(Id, ExpectedClosingBalance, actualClosingBalance, Variance.Value));
    }

    /// <summary>
    /// Supervisor verifies and approves the session close.
    /// </summary>
    public void SupervisorVerify(Guid supervisorUserId, string supervisorName, string? notes = null)
    {
        if (Status != StatusReconciled && Status != StatusShortage && Status != StatusOverage)
            throw new InvalidOperationException("Session must be closed before supervisor verification.");

        SupervisorUserId = supervisorUserId;
        SupervisorName = supervisorName;
        SupervisorVerificationTime = DateTime.UtcNow;

        if (notes is not null)
        {
            this.Notes = notes;
        }

        QueueDomainEvent(new TellerSessionVerified(Id, supervisorUserId, supervisorName));
    }

    /// <summary>
    /// Transfers cash to/from the vault or another teller.
    /// </summary>
    public void TransferCash(decimal amount, bool isTransferIn, string reference)
    {
        if (Status != StatusOpen && Status != StatusPaused)
            throw new InvalidOperationException("Cannot transfer cash on a closed session.");

        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be positive.", nameof(amount));

        if (isTransferIn)
        {
            // Receiving cash from vault
            TotalCashIn += amount;
        }
        else
        {
            // Sending cash to vault
            if (amount > ExpectedClosingBalance)
                throw new InvalidOperationException("Insufficient cash in drawer for this transfer.");
            TotalCashOut += amount;
        }

        ExpectedClosingBalance = OpeningBalance + TotalCashIn - TotalCashOut;
        TransactionCount++;

        QueueDomainEvent(new TellerSessionCashTransferred(Id, amount, isTransferIn, reference, ExpectedClosingBalance));
    }
}
