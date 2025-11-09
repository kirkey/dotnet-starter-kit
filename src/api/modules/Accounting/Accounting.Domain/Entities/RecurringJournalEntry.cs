using Accounting.Domain.Events.RecurringJournalEntry;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a template for journal entries that recur on a regular schedule (monthly accruals, depreciation, etc.).
/// </summary>
/// <remarks>
/// Use cases:
/// - Automate monthly recurring entries like rent, insurance, depreciation.
/// - Support standard accrual entries that repeat each period.
/// - Generate journal entries automatically based on schedule.
/// - Track recurring entry templates with activation and expiration dates.
/// - Maintain audit trail of entries generated from templates.
/// - Enable modification of templates without affecting historical entries.
/// - Support various frequencies: Monthly, Quarterly, Annually, Custom.
/// 
/// Default values:
/// - Frequency: Monthly (most common recurring pattern)
/// - IsActive: true (template is active and will generate entries)
/// - Status: Draft (new templates start as draft until approved)
/// - Amount: required (fixed amount for the recurring entry)
/// - StartDate: required (first date to generate entry)
/// - EndDate: null (continues indefinitely until manually stopped)
/// - NextRunDate: StartDate (calculated based on frequency)
/// - LastGeneratedDate: null (updated when entry is generated)
/// - GeneratedCount: 0 (tracks number of entries created)
/// 
/// Business rules:
/// - Must have balanced debit and credit lines
/// - Cannot generate entries before start date or after end date
/// - Frequency determines next run date calculation
/// - Only active templates generate entries
/// - Template must be approved before generating entries
/// - Generated entries link back to this template
/// - Can be suspended temporarily without deleting
/// 
/// Approval workflow:
/// - Uses AuditableEntityWithApproval for proper approval tracking
/// - ApprovedBy stores the user ID (Guid) of the approver
/// - ApproverName stores the readable name for display
/// - ApprovedOn stores the approval timestamp
/// </remarks>
public class RecurringJournalEntry : AuditableEntityWithApproval, IAggregateRoot
{
    /// <summary>
    /// Template code/identifier for this recurring entry.
    /// </summary>
    public string TemplateCode { get; private set; } = string.Empty;

    // Description property inherited from AuditableEntity base class

    /// <summary>
    /// Recurrence frequency: Monthly, Quarterly, Annually, Custom.
    /// </summary>
    public RecurrenceFrequency Frequency { get; private set; }

    /// <summary>
    /// Custom interval in days (used when Frequency is Custom).
    /// </summary>
    public int? CustomIntervalDays { get; private set; }

    /// <summary>
    /// Fixed amount for the recurring entry (for simple entries).
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Debit account for simple two-account entries.
    /// </summary>
    public DefaultIdType DebitAccountId { get; private set; }

    /// <summary>
    /// Credit account for simple two-account entries.
    /// </summary>
    public DefaultIdType CreditAccountId { get; private set; }

    /// <summary>
    /// Start date for generating recurring entries.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Optional end date when template expires.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Next scheduled date to generate an entry.
    /// </summary>
    public DateTime NextRunDate { get; private set; }

    /// <summary>
    /// Date when the last entry was generated from this template.
    /// </summary>
    public DateTime? LastGeneratedDate { get; private set; }

    /// <summary>
    /// Number of journal entries generated from this template.
    /// </summary>
    public int GeneratedCount { get; private set; }

    /// <summary>
    /// Whether the template is active and should generate entries.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Template status: Draft, Approved, Suspended, Expired.
    /// Note: Status is also inherited from AuditableEntityWithApproval for workflow tracking.
    /// This RecurringEntryStatus provides domain-specific states.
    /// </summary>
    public RecurringEntryStatus RecurringStatus { get; private set; }

    // ApprovedBy, ApproverName, and ApprovedOn are inherited from AuditableEntityWithApproval

    /// <summary>
    /// Optional reference to account posting batch.
    /// </summary>
    public DefaultIdType? PostingBatchId { get; init; }

    /// <summary>
    /// Optional memo text for generated entries.
    /// </summary>
    public string? Memo { get; private set; }

    private RecurringJournalEntry(
        string templateCode,
        string description,
        RecurrenceFrequency frequency,
        decimal amount,
        DefaultIdType debitAccountId,
        DefaultIdType creditAccountId,
        DateTime startDate,
        DateTime? endDate = null,
        int? customIntervalDays = null,
        string? memo = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(templateCode))
            throw new ArgumentException("Template code is required", nameof(templateCode));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        if (frequency == RecurrenceFrequency.Custom && (customIntervalDays == null || customIntervalDays <= 0))
            throw new ArgumentException("Custom interval days must be positive when using custom frequency", nameof(customIntervalDays));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be before start date", nameof(endDate));

        TemplateCode = templateCode.Trim();
        Description = description.Trim();
        Frequency = frequency;
        CustomIntervalDays = customIntervalDays;
        Amount = amount;
        DebitAccountId = debitAccountId;
        CreditAccountId = creditAccountId;
        StartDate = startDate;
        EndDate = endDate;
        NextRunDate = startDate;
        GeneratedCount = 0;
        IsActive = true;
        RecurringStatus = RecurringEntryStatus.Draft;
        Status = "Pending"; // Initialize workflow status from AuditableEntityWithApproval base class
        Memo = memo?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new RecurringJournalEntryCreated(Id, TemplateCode, Description, Frequency, Amount, StartDate));
    }

    /// <summary>
    /// Create a new recurring journal entry template.
    /// </summary>
    public static RecurringJournalEntry Create(
        string templateCode,
        string description,
        RecurrenceFrequency frequency,
        decimal amount,
        DefaultIdType debitAccountId,
        DefaultIdType creditAccountId,
        DateTime startDate,
        DateTime? endDate = null,
        int? customIntervalDays = null,
        string? memo = null,
        string? notes = null)
    {
        return new RecurringJournalEntry(templateCode, description, frequency, amount, 
            debitAccountId, creditAccountId, startDate, endDate, customIntervalDays, memo, notes);
    }

    /// <summary>
    /// Update template details (only allowed when not generating entries).
    /// </summary>
    public void Update(
        string? description = null,
        decimal? amount = null,
        DateTime? endDate = null,
        string? memo = null,
        string? notes = null)
    {
        if (RecurringStatus == RecurringEntryStatus.Expired)
            throw new RecurringJournalEntryExpiredException(Id);

        if (!string.IsNullOrWhiteSpace(description))
            Description = description.Trim();

        if (amount.HasValue)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            Amount = amount.Value;
        }

        if (endDate.HasValue)
        {
            if (endDate.Value < StartDate)
                throw new ArgumentException("End date cannot be before start date", nameof(endDate));
            EndDate = endDate.Value;
        }

        Memo = memo?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new RecurringJournalEntryUpdated(Id, Description, Amount));
    }

    /// <summary>
    /// Approve the template to allow entry generation.
    /// </summary>
    /// <param name="approvedById">The user ID of the approver.</param>
    /// <param name="approverName">Optional readable name of the approver.</param>
    public void Approve(DefaultIdType approvedById, string? approverName = null)
    {
        if (RecurringStatus == RecurringEntryStatus.Approved)
            throw new RecurringJournalEntryAlreadyApprovedException(Id);

        if (RecurringStatus == RecurringEntryStatus.Expired)
            throw new RecurringJournalEntryExpiredException(Id);

        RecurringStatus = RecurringEntryStatus.Approved;
        Status = "Approved"; // Update workflow status from base class
        ApprovedBy = approvedById;
        ApproverName = approverName?.Trim();
        ApprovedOn = DateTime.UtcNow;

        QueueDomainEvent(new RecurringJournalEntryApproved(Id, approvedById.ToString(), ApprovedOn.Value));
    }

    /// <summary>
    /// Temporarily suspend entry generation.
    /// </summary>
    public void Suspend(string? reason = null)
    {
        if (RecurringStatus == RecurringEntryStatus.Expired)
            throw new RecurringJournalEntryExpiredException(Id);

        RecurringStatus = RecurringEntryStatus.Suspended;
        IsActive = false;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Suspended: {reason}" 
                : $"{Notes}\nSuspended: {reason}";
        }

        QueueDomainEvent(new RecurringJournalEntrySuspended(Id, reason));
    }

    /// <summary>
    /// Reactivate a suspended template.
    /// </summary>
    public void Reactivate()
    {
        if (RecurringStatus != RecurringEntryStatus.Suspended)
            throw new InvalidRecurringEntryStatusException($"Cannot reactivate template with status {RecurringStatus}");

        RecurringStatus = RecurringEntryStatus.Approved;
        IsActive = true;

        QueueDomainEvent(new RecurringJournalEntryReactivated(Id));
    }

    /// <summary>
    /// Record that a journal entry was generated from this template.
    /// </summary>
    public void RecordGeneration(DefaultIdType journalEntryId, DateTime generatedDate)
    {
        if (RecurringStatus != RecurringEntryStatus.Approved)
            throw new InvalidRecurringEntryStatusException($"Cannot generate entries with status {RecurringStatus}");

        if (!IsActive)
            throw new RecurringJournalEntryInactiveException(Id);

        LastGeneratedDate = generatedDate;
        GeneratedCount++;
        NextRunDate = CalculateNextRunDate(generatedDate);

        // Check if template should expire
        if (EndDate.HasValue && NextRunDate > EndDate.Value)
        {
            RecurringStatus = RecurringEntryStatus.Expired;
            IsActive = false;
            QueueDomainEvent(new RecurringJournalEntryExpired(Id));
        }

        QueueDomainEvent(new RecurringJournalEntryGenerated(Id, journalEntryId, generatedDate, NextRunDate));
    }

    /// <summary>
    /// Calculate the next run date based on frequency.
    /// </summary>
    private DateTime CalculateNextRunDate(DateTime currentDate)
    {
        return Frequency switch
        {
            RecurrenceFrequency.Monthly => currentDate.AddMonths(1),
            RecurrenceFrequency.Quarterly => currentDate.AddMonths(3),
            RecurrenceFrequency.Annually => currentDate.AddYears(1),
            RecurrenceFrequency.Custom => currentDate.AddDays(CustomIntervalDays!.Value),
            _ => throw new InvalidRecurrenceFrequencyException($"Invalid recurrence frequency: {Frequency}")
        };
    }
}

/// <summary>
/// Recurrence frequency options.
/// </summary>
public enum RecurrenceFrequency
{
    Weekly,
    Monthly,
    Quarterly,
    Annually,
    Yearly,
    Custom
}

/// <summary>
/// Recurring entry status values.
/// </summary>
public enum RecurringEntryStatus
{
    Draft,
    Approved,
    Suspended,
    Expired
}
