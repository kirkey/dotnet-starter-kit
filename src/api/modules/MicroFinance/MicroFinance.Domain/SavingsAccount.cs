using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a savings account owned by a member.
/// </summary>
/// <remarks>
/// <para><strong>What is this?</strong></para>
/// <para>
/// A SavingsAccount is a deposit account that allows members to save money, earn interest,
/// and access their funds. Savings accounts are fundamental to microfinance operations as they
/// help mobilize local deposits to fund lending activities while providing members a safe
/// place to store their money.
/// </para>
/// 
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record deposits and withdrawals for member savings</description></item>
///   <item><description>Calculate and post interest earnings periodically</description></item>
///   <item><description>Track account status and lifecycle (Pending → Active → Dormant → Closed)</description></item>
///   <item><description>Freeze accounts for suspicious activity or legal holds</description></item>
///   <item><description>Link to fixed deposits for automatic transfers at maturity</description></item>
///   <item><description>Support compulsory savings linked to loan disbursements</description></item>
///   <item><description>Generate account statements for members</description></item>
///   <item><description>Track dormant accounts for regulatory compliance</description></item>
/// </list>
/// 
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Savings accounts are fundamental to microfinance operations. They:
/// - Mobilize local deposits to fund lending operations
/// - Provide members with a safe place to save
/// - Build financial discipline (especially compulsory savings linked to loans)
/// - Generate liquidity data for institutional planning
/// </para>
/// <para>
/// Account status progression: Pending → Active → (Dormant after inactivity) → Closed.
/// Frozen status is a temporary hold that can be applied at any time for investigations.
/// </para>
/// 
/// <para><strong>Account Types (via SavingsProduct):</strong></para>
/// <list type="bullet">
///   <item><description><strong>Regular Savings</strong>: Standard savings with flexible deposits/withdrawals</description></item>
///   <item><description><strong>Compulsory Savings</strong>: Required savings linked to loan products</description></item>
///   <item><description><strong>Children's Savings</strong>: Special accounts for minors with guardian oversight</description></item>
///   <item><description><strong>Group Savings</strong>: Collective savings for solidarity groups</description></item>
///   <item><description><strong>Target Savings</strong>: Goal-based savings with withdrawal restrictions</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountNumber: Required, unique, max 64 characters</description></item>
///   <item><description>MemberId: Required, must reference an active member</description></item>
///   <item><description>SavingsProductId: Required, must reference an active savings product</description></item>
///   <item><description>Balance: Cannot be negative (enforced by business rules)</description></item>
///   <item><description>Withdrawals: Cannot exceed available balance</description></item>
///   <item><description>Deposits/Withdrawals: Must be positive amounts</description></item>
///   <item><description>Account closure: Balance must be zero</description></item>
/// </list>
/// 
/// <para><strong>Required Permissions:</strong></para>
/// <list type="bullet">
///   <item><description><c>MicroFinance.Create</c> - Open new savings accounts</description></item>
///   <item><description><c>MicroFinance.View</c> - View account details and transactions</description></item>
///   <item><description><c>MicroFinance.Deposit</c> - Make deposits to accounts</description></item>
///   <item><description><c>MicroFinance.Withdraw</c> - Make withdrawals from accounts</description></item>
///   <item><description><c>MicroFinance.Transfer</c> - Transfer between accounts</description></item>
///   <item><description><c>MicroFinance.Freeze</c> - Freeze/unfreeze accounts</description></item>
///   <item><description><c>MicroFinance.Close</c> - Close savings accounts</description></item>
///   <item><description><c>MicroFinance.PostInterest</c> - Post interest to accounts</description></item>
/// </list>
/// 
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="SavingsProduct"/> - Product template defining account terms</description></item>
///   <item><description><see cref="Member"/> - Account owner</description></item>
///   <item><description><see cref="SavingsTransaction"/> - Individual transactions (deposits, withdrawals, interest)</description></item>
///   <item><description><see cref="FixedDeposit"/> - May link for maturity transfers</description></item>
///   <item><description><see cref="FeeCharge"/> - Account maintenance fees</description></item>
/// </list>
/// 
/// <para><strong>Default Values:</strong></para>
/// <list type="bullet">
///   <item><description>Status: "Pending" (if opening balance is 0), "Active" (if opening balance > 0)</description></item>
///   <item><description>Balance: 0 or opening balance amount</description></item>
///   <item><description>TotalDeposits: Equal to opening balance (if any)</description></item>
///   <item><description>TotalWithdrawals: 0</description></item>
///   <item><description>TotalInterestEarned: 0</description></item>
///   <item><description>OpenedDate: Current UTC date (if not specified)</description></item>
/// </list>
/// 
/// <para><strong>JSON Example (Create Request):</strong></para>
/// <code>
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "savingsProductId": "7fa85f64-5717-4562-b3fc-2c963f66afa9",
///   "accountNumber": "SAV-2024-001234",
///   "openingBalance": 1000.00,
///   "notes": "Regular savings account for emergency fund"
/// }
/// </code>
/// 
/// <para><strong>JSON Example (Response):</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "accountNumber": "SAV-2024-001234",
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "memberName": "John Doe",
///   "savingsProductId": "7fa85f64-5717-4562-b3fc-2c963f66afa9",
///   "savingsProductName": "Regular Savings",
///   "balance": 1000.00,
///   "totalDeposits": 1000.00,
///   "totalWithdrawals": 0.00,
///   "totalInterestEarned": 0.00,
///   "openedDate": "2024-01-15",
///   "status": "Active",
///   "interestRate": 4.5
/// }
/// </code>
/// 
/// <para><strong>JSON Example (Deposit Request):</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "amount": 500.00,
///   "reference": "Cash deposit - January savings",
///   "transactionDate": "2024-01-20"
/// }
/// </code>
/// 
/// <para><strong>JSON Example (Withdrawal Request):</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "amount": 200.00,
///   "reference": "ATM withdrawal",
///   "transactionDate": "2024-01-25"
/// }
/// </code>
/// </remarks>
public class SavingsAccount : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for account number. (2^6 = 64)</summary>
    public const int AccountNumberMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    // Account Statuses
    public const string StatusPending = "Pending";
    public const string StatusActive = "Active";
    public const string StatusDormant = "Dormant";
    public const string StatusClosed = "Closed";
    public const string StatusFrozen = "Frozen";

    /// <summary>Gets the unique account number.</summary>
    public string AccountNumber { get; private set; } = default!;

    /// <summary>Gets the member ID who owns this account.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the savings product ID.</summary>
    public DefaultIdType SavingsProductId { get; private set; }

    /// <summary>Gets the savings product navigation property.</summary>
    public virtual SavingsProduct SavingsProduct { get; private set; } = default!;

    /// <summary>Gets the current balance.</summary>
    public decimal Balance { get; private set; }

    /// <summary>Gets the total deposits made.</summary>
    public decimal TotalDeposits { get; private set; }

    /// <summary>Gets the total withdrawals made.</summary>
    public decimal TotalWithdrawals { get; private set; }

    /// <summary>Gets the total interest earned.</summary>
    public decimal TotalInterestEarned { get; private set; }

    /// <summary>Gets the date the account was opened.</summary>
    public DateOnly OpenedDate { get; private set; }

    /// <summary>Gets the date the account was closed.</summary>
    public DateOnly? ClosedDate { get; private set; }

    /// <summary>Gets the last interest posting date.</summary>
    public DateOnly? LastInterestPostingDate { get; private set; }

    /// <summary>Gets the current status of the account.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the collection of transactions for this account.</summary>
    public virtual ICollection<SavingsTransaction> Transactions { get; private set; } = new List<SavingsTransaction>();

    private SavingsAccount() { }

    private SavingsAccount(
        DefaultIdType id,
        string accountNumber,
        DefaultIdType memberId,
        DefaultIdType savingsProductId,
        decimal openingBalance,
        DateOnly openedDate,
        string? notes)
    {
        Id = id;
        AccountNumber = accountNumber.Trim();
        MemberId = memberId;
        SavingsProductId = savingsProductId;
        Balance = openingBalance;
        TotalDeposits = openingBalance > 0 ? openingBalance : 0;
        TotalWithdrawals = 0;
        TotalInterestEarned = 0;
        OpenedDate = openedDate;
        Status = StatusActive;
        Notes = notes?.Trim();

        QueueDomainEvent(new SavingsAccountCreated { SavingsAccount = this });
    }

    /// <summary>
    /// Creates a new SavingsAccount instance.
    /// </summary>
    public static SavingsAccount Create(
        string accountNumber,
        DefaultIdType memberId,
        DefaultIdType savingsProductId,
        decimal openingBalance = 0,
        DateOnly? openedDate = null,
        string? notes = null)
    {
        var account = new SavingsAccount(
            DefaultIdType.NewGuid(),
            accountNumber,
            memberId,
            savingsProductId,
            openingBalance,
            openedDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            notes);
        
        // Set status to Pending for new accounts that need activation
        if (openingBalance == 0)
        {
            account.Status = StatusPending;
        }
        
        return account;
    }

    /// <summary>
    /// Activates a pending account.
    /// </summary>
    public SavingsAccount Activate()
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot activate account in {Status} status. Only Pending accounts can be activated.");

        Status = StatusActive;
        QueueDomainEvent(new SavingsAccountActivated { AccountId = Id });
        return this;
    }

    /// <summary>
    /// Records a deposit to the account.
    /// </summary>
    public SavingsAccount Deposit(decimal amount)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot deposit to account in {Status} status.");

        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        Balance += amount;
        TotalDeposits += amount;

        QueueDomainEvent(new SavingsDeposited { AccountId = Id, Amount = amount });
        return this;
    }

    /// <summary>
    /// Records a withdrawal from the account.
    /// </summary>
    public SavingsAccount Withdraw(decimal amount)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot withdraw from account in {Status} status.");

        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance for withdrawal.");

        Balance -= amount;
        TotalWithdrawals += amount;

        QueueDomainEvent(new SavingsWithdrawn { AccountId = Id, Amount = amount });
        return this;
    }

    /// <summary>
    /// Posts interest to the account.
    /// </summary>
    public SavingsAccount PostInterest(decimal interestAmount)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot post interest to account in {Status} status.");

        if (interestAmount <= 0)
            throw new ArgumentException("Interest amount must be positive.", nameof(interestAmount));

        Balance += interestAmount;
        TotalInterestEarned += interestAmount;
        LastInterestPostingDate = DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new SavingsInterestPosted { AccountId = Id, Amount = interestAmount });
        return this;
    }

    /// <summary>
    /// Freezes the account.
    /// </summary>
    public SavingsAccount Freeze(string? reason = null)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot freeze account in {Status} status.");

        Status = StatusFrozen;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Frozen: {reason}" : $"{Notes}\nFrozen: {reason}";
        }

        return this;
    }

    /// <summary>
    /// Unfreezes the account.
    /// </summary>
    public SavingsAccount Unfreeze()
    {
        if (Status != StatusFrozen)
            throw new InvalidOperationException($"Cannot unfreeze account in {Status} status.");

        Status = StatusActive;
        return this;
    }

    /// <summary>
    /// Closes the account.
    /// </summary>
    public SavingsAccount Close(string? reason = null)
    {
        if (Status == StatusClosed)
            throw new InvalidOperationException("Account is already closed.");

        if (Balance != 0)
            throw new InvalidOperationException("Cannot close account with non-zero balance.");

        Status = StatusClosed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Closed: {reason}" : $"{Notes}\nClosed: {reason}";
        }

        QueueDomainEvent(new SavingsAccountClosed { AccountId = Id });
        return this;
    }
}

