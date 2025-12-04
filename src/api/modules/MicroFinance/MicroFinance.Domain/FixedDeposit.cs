using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a fixed/term deposit account in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Accept time-bound deposits with higher interest rates than regular savings</description></item>
///   <item><description>Track deposit maturity and automatic renewal/payout</description></item>
///   <item><description>Handle premature withdrawals with penalty calculations</description></item>
///   <item><description>Post periodic interest to linked savings accounts</description></item>
///   <item><description>Issue deposit certificates for member records</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Fixed deposits (also called term deposits or time deposits) are a key liability product for MFIs:
/// </para>
/// <list type="bullet">
///   <item><description>Provide predictable, stable funding with known maturity dates</description></item>
///   <item><description>Offer higher interest rates in exchange for locked funds</description></item>
///   <item><description>Help match loan tenors with deposit tenors for ALM (Asset-Liability Management)</description></item>
///   <item><description>Build member savings discipline through commitment</description></item>
/// </list>
/// <para>
/// Status progression: Pending → Active → (Matured | PrematurelyClosed) → (Renewed).
/// At maturity, the system executes the configured maturity instruction.
/// </para>
/// <para><strong>Maturity Instructions:</strong></para>
/// <list type="bullet">
///   <item><description><strong>RenewPrincipalAndInterest</strong>: Roll over entire amount into new term</description></item>
///   <item><description><strong>RenewPrincipal</strong>: Renew principal, pay out interest</description></item>
///   <item><description><strong>TransferToSavings</strong>: Move all funds to linked savings account</description></item>
///   <item><description><strong>PayOut</strong>: Issue payment to member</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - Deposit owner</description></item>
///   <item><description><see cref="SavingsProduct"/> - May reference for interest rate tiers</description></item>
///   <item><description><see cref="SavingsAccount"/> - Linked account for interest/maturity transfers</description></item>
/// </list>
/// </remarks>
public class FixedDeposit : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for certificate number field. (2^6 = 64)</summary>
    public const int CertificateNumberMaxLength = 64;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for maturity instruction field. (2^5 = 32)</summary>
    public const int MaturityInstructionMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    // Fixed Deposit Statuses
    public const string StatusPending = "Pending";
    public const string StatusActive = "Active";
    public const string StatusMatured = "Matured";
    public const string StatusPrematurelyClosed = "PrematurelyClosed";
    public const string StatusRenewed = "Renewed";

    // Maturity Instructions
    public const string MaturityRenewPrincipalAndInterest = "RenewPrincipalAndInterest";
    public const string MaturityRenewPrincipal = "RenewPrincipal";
    public const string MaturityTransferToSavings = "TransferToSavings";
    public const string MaturityPayOut = "PayOut";

    /// <summary>Gets the unique certificate number.</summary>
    public string CertificateNumber { get; private set; } = default!;

    /// <summary>Gets the member ID who owns this deposit.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the savings product ID (for interest rates and terms).</summary>
    public DefaultIdType? SavingsProductId { get; private set; }

    /// <summary>Gets the savings product navigation property.</summary>
    public virtual SavingsProduct? SavingsProduct { get; private set; }

    /// <summary>Gets the linked savings account for interest transfer.</summary>
    public DefaultIdType? LinkedSavingsAccountId { get; private set; }

    /// <summary>Gets the linked savings account navigation property.</summary>
    public virtual SavingsAccount? LinkedSavingsAccount { get; private set; }

    /// <summary>Gets the principal amount deposited.</summary>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>Gets the annual interest rate.</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Gets the term in months.</summary>
    public int TermMonths { get; private set; }

    /// <summary>Gets the deposit date.</summary>
    public DateOnly DepositDate { get; private set; }

    /// <summary>Gets the maturity date.</summary>
    public DateOnly MaturityDate { get; private set; }

    /// <summary>Gets the interest earned so far.</summary>
    public decimal InterestEarned { get; private set; }

    /// <summary>Gets the interest paid out so far.</summary>
    public decimal InterestPaid { get; private set; }

    /// <summary>Gets the maturity instruction.</summary>
    public string MaturityInstruction { get; private set; } = default!;

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the date the deposit was closed.</summary>
    public DateOnly? ClosedDate { get; private set; }

    private FixedDeposit() { }

    private FixedDeposit(
        DefaultIdType id,
        string certificateNumber,
        DefaultIdType memberId,
        DefaultIdType? savingsProductId,
        DefaultIdType? linkedSavingsAccountId,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        DateOnly depositDate,
        string maturityInstruction,
        string? notes)
    {
        Id = id;
        CertificateNumber = certificateNumber.Trim();
        MemberId = memberId;
        SavingsProductId = savingsProductId;
        LinkedSavingsAccountId = linkedSavingsAccountId;
        PrincipalAmount = principalAmount;
        InterestRate = interestRate;
        TermMonths = termMonths;
        DepositDate = depositDate;
        MaturityDate = depositDate.AddMonths(termMonths);
        InterestEarned = 0;
        InterestPaid = 0;
        MaturityInstruction = maturityInstruction.Trim();
        Status = StatusActive;
        Notes = notes?.Trim();

        QueueDomainEvent(new FixedDepositCreated { FixedDeposit = this });
    }

    /// <summary>
    /// Creates a new FixedDeposit instance.
    /// </summary>
    public static FixedDeposit Create(
        string certificateNumber,
        DefaultIdType memberId,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        DefaultIdType? savingsProductId = null,
        DefaultIdType? linkedSavingsAccountId = null,
        DateOnly? depositDate = null,
        string? maturityInstruction = null,
        string? notes = null)
    {
        return new FixedDeposit(
            DefaultIdType.NewGuid(),
            certificateNumber,
            memberId,
            savingsProductId,
            linkedSavingsAccountId,
            principalAmount,
            interestRate,
            termMonths,
            depositDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            maturityInstruction ?? MaturityTransferToSavings,
            notes);
    }

    /// <summary>
    /// Posts interest earned to the deposit.
    /// </summary>
    public FixedDeposit PostInterest(decimal interestAmount)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot post interest to deposit in {Status} status.");

        if (interestAmount <= 0)
            throw new ArgumentException("Interest amount must be positive.", nameof(interestAmount));

        InterestEarned += interestAmount;
        QueueDomainEvent(new FixedDepositInterestPosted { DepositId = Id, Amount = interestAmount });
        return this;
    }

    /// <summary>
    /// Records interest payout.
    /// </summary>
    public FixedDeposit PayInterest(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payout amount must be positive.", nameof(amount));

        if (amount > InterestEarned - InterestPaid)
            throw new InvalidOperationException("Payout amount exceeds available interest.");

        InterestPaid += amount;
        return this;
    }

    /// <summary>
    /// Matures the fixed deposit.
    /// </summary>
    public FixedDeposit Mature()
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot mature deposit in {Status} status.");

        Status = StatusMatured;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new FixedDepositMatured { DepositId = Id });
        return this;
    }

    /// <summary>
    /// Renews the fixed deposit.
    /// </summary>
    public FixedDeposit Renew(int? newTermMonths = null, decimal? newInterestRate = null)
    {
        if (Status != StatusMatured)
            throw new InvalidOperationException($"Cannot renew deposit in {Status} status.");

        DepositDate = DateOnly.FromDateTime(DateTime.UtcNow);
        TermMonths = newTermMonths ?? TermMonths;
        InterestRate = newInterestRate ?? InterestRate;
        MaturityDate = DepositDate.AddMonths(TermMonths);
        Status = StatusRenewed;
        ClosedDate = null;

        QueueDomainEvent(new FixedDepositRenewed { DepositId = Id });
        return this;
    }

    /// <summary>
    /// Closes the deposit prematurely.
    /// </summary>
    public FixedDeposit ClosePremature(string? reason = null)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot close deposit in {Status} status.");

        Status = StatusPrematurelyClosed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Premature closure: {reason}" : $"{Notes}\nPremature closure: {reason}";
        }

        QueueDomainEvent(new FixedDepositPrematurelyClosed { DepositId = Id, Reason = reason });
        return this;
    }

    /// <summary>
    /// Updates the maturity instruction.
    /// </summary>
    public FixedDeposit UpdateMaturityInstruction(string instruction)
    {
        if (Status == StatusMatured || Status == StatusPrematurelyClosed)
            throw new InvalidOperationException($"Cannot update instruction for deposit in {Status} status.");

        MaturityInstruction = instruction.Trim();
        return this;
    }
}
