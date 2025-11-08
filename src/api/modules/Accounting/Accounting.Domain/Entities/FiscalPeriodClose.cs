using Accounting.Domain.Events.FiscalPeriodClose;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a fiscal period close process with validation, checklist management, and finalization workflow for ensuring proper period-end accounting.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage month-end, quarter-end, and year-end close processes.
/// - Track completion of required close tasks and validations.
/// - Ensure all transactions posted before closing period.
/// - Generate closing entries for revenue/expense accounts.
/// - Transfer net income to retained earnings (year-end).
/// - Lock period to prevent backdated transactions.
/// - Support audit trail for close process.
/// - Enable reopening of closed periods with authorization.
/// 
/// Default values:
/// - CloseNumber: required unique identifier (example: "CLOSE-2025-10")
/// - PeriodId: required accounting period reference
/// - CloseType: required (MonthEnd, QuarterEnd, YearEnd)
/// - Status: "InProgress" (close process initiated)
/// - IsComplete: false (not yet finalized)
/// - RequiredTasksComplete: false (checklist in progress)
/// 
/// Business rules:
/// - All required tasks must be complete before finalizing
/// - Trial balance must be balanced
/// - All journals must be posted
/// - Bank reconciliations must be complete
/// - No unapproved transactions allowed
/// - Period cannot be closed if subsequent period is closed
/// - Year-end requires transfer of net income to retained earnings
/// - Reopening requires proper authorization and reason
/// </remarks>
public class FiscalPeriodClose : AuditableEntity, IAggregateRoot
{
    public string CloseNumber { get; private set; } = string.Empty;
    public DefaultIdType PeriodId { get; private set; }
    public string CloseType { get; private set; } = string.Empty; // MonthEnd, QuarterEnd, YearEnd
    public DateTime PeriodStartDate { get; private set; }
    public DateTime PeriodEndDate { get; private set; }
    public DateTime CloseInitiatedDate { get; private set; }
    public string InitiatedBy { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty; // InProgress, Completed, Reopened
    public bool IsComplete { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string? CompletedBy { get; private set; }
    public bool RequiredTasksComplete { get; private set; }
    public int TasksCompleted { get; private set; }
    public int TasksRemaining { get; private set; }
    public bool TrialBalanceGenerated { get; private set; }
    public bool TrialBalanceBalanced { get; private set; }
    public bool AllJournalsPosted { get; private set; }
    public bool BankReconciliationsComplete { get; private set; }
    public bool ApReconciliationComplete { get; private set; }
    public bool ArReconciliationComplete { get; private set; }
    public bool InventoryReconciliationComplete { get; private set; }
    public bool FixedAssetDepreciationPosted { get; private set; }
    public bool PrepaidExpensesAmortized { get; private set; }
    public bool AccrualsPosted { get; private set; }
    public bool IntercompanyReconciled { get; private set; }
    public bool NetIncomeTransferred { get; private set; } // Year-end only
    public DefaultIdType? TrialBalanceId { get; private set; }
    public decimal? FinalNetIncome { get; private set; }
    public string? ReopenReason { get; private set; }
    public DateTime? ReopenedDate { get; private set; }
    public string? ReopenedBy { get; private set; }

    private readonly List<CloseTaskItem> _tasks = new();
    public IReadOnlyCollection<CloseTaskItem> Tasks => _tasks.AsReadOnly();

    private readonly List<CloseValidationIssue> _validationIssues = new();
    public IReadOnlyCollection<CloseValidationIssue> ValidationIssues => _validationIssues.AsReadOnly();

    private FiscalPeriodClose() { Status = "InProgress"; }

    private FiscalPeriodClose(string closeNumber, DefaultIdType periodId, string closeType,
        DateTime periodStartDate, DateTime periodEndDate, string initiatedBy,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(closeNumber))
            throw new ArgumentException("Close number is required", nameof(closeNumber));
        if (string.IsNullOrWhiteSpace(closeType))
            throw new ArgumentException("Close type is required", nameof(closeType));
        if (string.IsNullOrWhiteSpace(initiatedBy))
            throw new ArgumentException("Initiator is required", nameof(initiatedBy));

        var validCloseTypes = new[] { "MonthEnd", "QuarterEnd", "YearEnd" };
        if (!validCloseTypes.Contains(closeType))
            throw new ArgumentException($"Close type must be one of: {string.Join(", ", validCloseTypes)}", nameof(closeType));

        CloseNumber = closeNumber.Trim();
        Name = closeNumber.Trim();
        PeriodId = periodId;
        CloseType = closeType.Trim();
        PeriodStartDate = periodStartDate;
        PeriodEndDate = periodEndDate;
        CloseInitiatedDate = DateTime.UtcNow;
        InitiatedBy = initiatedBy.Trim();
        Status = "InProgress";
        IsComplete = false;
        RequiredTasksComplete = false;
        Description = description?.Trim();
        Notes = notes?.Trim();

        InitializeStandardTasks();

        QueueDomainEvent(new FiscalPeriodCloseInitiated(Id, CloseNumber, PeriodId, CloseType, InitiatedBy, CloseInitiatedDate));
    }

    public static FiscalPeriodClose Create(string closeNumber, DefaultIdType periodId, string closeType,
        DateTime periodStartDate, DateTime periodEndDate, string initiatedBy,
        string? description = null, string? notes = null)
    {
        return new FiscalPeriodClose(closeNumber, periodId, closeType, periodStartDate, periodEndDate,
            initiatedBy, description, notes);
    }

    private void InitializeStandardTasks()
    {
        _tasks.Add(CloseTaskItem.Create("Generate Trial Balance", true));
        _tasks.Add(CloseTaskItem.Create("Verify Trial Balance Balanced", true));
        _tasks.Add(CloseTaskItem.Create("Post All Journal Entries", true));
        _tasks.Add(CloseTaskItem.Create("Complete Bank Reconciliations", true));
        _tasks.Add(CloseTaskItem.Create("Reconcile AP Subsidiary Ledger", true));
        _tasks.Add(CloseTaskItem.Create("Reconcile AR Subsidiary Ledger", true));
        _tasks.Add(CloseTaskItem.Create("Post Fixed Asset Depreciation", CloseType == "MonthEnd" || CloseType == "YearEnd"));
        _tasks.Add(CloseTaskItem.Create("Amortize Prepaid Expenses", true));
        _tasks.Add(CloseTaskItem.Create("Post Accruals", true));
        _tasks.Add(CloseTaskItem.Create("Reconcile Inter-company Transactions", false));
        _tasks.Add(CloseTaskItem.Create("Reconcile Inventory", CloseType == "YearEnd"));

        if (CloseType == "YearEnd")
        {
            _tasks.Add(CloseTaskItem.Create("Transfer Net Income to Retained Earnings", true));
            _tasks.Add(CloseTaskItem.Create("Post Closing Entries", true));
        }

        TasksRemaining = _tasks.Count(t => t.IsRequired && !t.IsComplete);
    }

    public FiscalPeriodClose CompleteTask(string taskName)
    {
        if (Status == "Completed")
            throw new InvalidOperationException("Cannot modify completed period close");

        var task = _tasks.FirstOrDefault(t => t.TaskName == taskName);
        if (task == null)
            throw new ArgumentException($"Task '{taskName}' not found", nameof(taskName));

        task.Complete();
        UpdateTaskMetrics();

        QueueDomainEvent(new FiscalPeriodCloseTaskCompleted(Id, CloseNumber, taskName));
        return this;
    }

    public FiscalPeriodClose AddValidationIssue(string issueDescription, string severity, string? resolution = null)
    {
        var issue = CloseValidationIssue.Create(issueDescription, severity, resolution);
        _validationIssues.Add(issue);

        QueueDomainEvent(new FiscalPeriodCloseValidationIssueFound(Id, CloseNumber, issueDescription, severity));
        return this;
    }

    public FiscalPeriodClose ResolveValidationIssue(string issueDescription, string resolution)
    {
        var issue = _validationIssues.FirstOrDefault(i => i.IssueDescription == issueDescription && !i.IsResolved);
        if (issue == null)
            throw new ArgumentException($"Unresolved issue '{issueDescription}' not found", nameof(issueDescription));

        issue.Resolve(resolution);

        QueueDomainEvent(new FiscalPeriodCloseValidationIssueResolved(Id, CloseNumber, issueDescription, resolution));
        return this;
    }

    public FiscalPeriodClose SetTrialBalance(DefaultIdType trialBalanceId, bool isBalanced)
    {
        TrialBalanceId = trialBalanceId;
        TrialBalanceGenerated = true;
        TrialBalanceBalanced = isBalanced;

        CompleteTask("Generate Trial Balance");
        if (isBalanced)
            CompleteTask("Verify Trial Balance Balanced");

        return this;
    }

    public FiscalPeriodClose SetNetIncome(decimal netIncome)
    {
        FinalNetIncome = netIncome;
        return this;
    }

    public FiscalPeriodClose MarkJournalsPosted()
    {
        AllJournalsPosted = true;
        CompleteTask("Post All Journal Entries");
        return this;
    }

    public FiscalPeriodClose MarkBankReconciliationsComplete()
    {
        BankReconciliationsComplete = true;
        CompleteTask("Complete Bank Reconciliations");
        return this;
    }

    public FiscalPeriodClose MarkApReconciliationComplete()
    {
        ApReconciliationComplete = true;
        CompleteTask("Reconcile AP Subsidiary Ledger");
        return this;
    }

    public FiscalPeriodClose MarkArReconciliationComplete()
    {
        ArReconciliationComplete = true;
        CompleteTask("Reconcile AR Subsidiary Ledger");
        return this;
    }

    public FiscalPeriodClose MarkDepreciationPosted()
    {
        FixedAssetDepreciationPosted = true;
        CompleteTask("Post Fixed Asset Depreciation");
        return this;
    }

    public FiscalPeriodClose MarkPrepaidsAmortized()
    {
        PrepaidExpensesAmortized = true;
        CompleteTask("Amortize Prepaid Expenses");
        return this;
    }

    public FiscalPeriodClose MarkAccrualsPosted()
    {
        AccrualsPosted = true;
        CompleteTask("Post Accruals");
        return this;
    }

    public FiscalPeriodClose MarkIntercompanyReconciled()
    {
        IntercompanyReconciled = true;
        CompleteTask("Reconcile Inter-company Transactions");
        return this;
    }

    public FiscalPeriodClose MarkNetIncomeTransferred()
    {
        if (CloseType != "YearEnd")
            throw new InvalidOperationException("Net income transfer only applicable for year-end close");

        NetIncomeTransferred = true;
        CompleteTask("Transfer Net Income to Retained Earnings");
        return this;
    }

    private void UpdateTaskMetrics()
    {
        TasksCompleted = _tasks.Count(t => t.IsComplete);
        TasksRemaining = _tasks.Count(t => t.IsRequired && !t.IsComplete);
        RequiredTasksComplete = TasksRemaining == 0;
    }

    public FiscalPeriodClose Complete(string completedBy)
    {
        if (string.IsNullOrWhiteSpace(completedBy))
            throw new ArgumentException("Completer information is required", nameof(completedBy));

        if (Status == "Completed")
            throw new InvalidOperationException("Period close is already completed");

        if (!RequiredTasksComplete)
            throw new InvalidOperationException($"Cannot complete period close with {TasksRemaining} required tasks remaining");

        if (!TrialBalanceBalanced)
            throw new InvalidOperationException("Cannot complete period close with unbalanced trial balance");

        if (_validationIssues.Any(i => !i.IsResolved && i.Severity == "Critical"))
            throw new InvalidOperationException("Cannot complete period close with unresolved critical validation issues");

        if (CloseType == "YearEnd" && !NetIncomeTransferred)
            throw new InvalidOperationException("Cannot complete year-end close without transferring net income");

        Status = "Completed";
        IsComplete = true;
        CompletedDate = DateTime.UtcNow;
        CompletedBy = completedBy.Trim();

        QueueDomainEvent(new FiscalPeriodCloseCompleted(Id, CloseNumber, PeriodId, CloseType, CompletedBy, CompletedDate.Value, FinalNetIncome));
        return this;
    }

    public FiscalPeriodClose Reopen(string reopenedBy, string reason)
    {
        if (string.IsNullOrWhiteSpace(reopenedBy))
            throw new ArgumentException("Reopener information is required", nameof(reopenedBy));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reopen reason is required", nameof(reason));

        if (Status != "Completed")
            throw new InvalidOperationException("Can only reopen completed period closes");

        Status = "Reopened";
        IsComplete = false;
        ReopenReason = reason.Trim();
        ReopenedDate = DateTime.UtcNow;
        ReopenedBy = reopenedBy.Trim();
        Notes = $"{Notes}\n\nReopened: {reason.Trim()}".Trim();

        QueueDomainEvent(new FiscalPeriodCloseReopened(Id, CloseNumber, PeriodId, ReopenedBy, reason));
        return this;
    }

    public decimal CompletionPercentage => _tasks.Any()
        ? ((decimal)TasksCompleted / _tasks.Count) * 100
        : 0m;

    public bool HasUnresolvedCriticalIssues => _validationIssues.Any(i => !i.IsResolved && i.Severity == "Critical");
}

public class CloseTaskItem
{
    public string TaskName { get; private set; } = string.Empty;
    public bool IsRequired { get; private set; }
    public bool IsComplete { get; private set; }
    public DateTime? CompletedDate { get; private set; }

    private CloseTaskItem() { }

    private CloseTaskItem(string taskName, bool isRequired)
    {
        TaskName = taskName;
        IsRequired = isRequired;
        IsComplete = false;
    }

    public static CloseTaskItem Create(string taskName, bool isRequired)
    {
        return new CloseTaskItem(taskName, isRequired);
    }

    public void Complete()
    {
        IsComplete = true;
        CompletedDate = DateTime.UtcNow;
    }
}

public class CloseValidationIssue
{
    public string IssueDescription { get; private set; } = string.Empty;
    public string Severity { get; private set; } = string.Empty; // Critical, Warning, Info
    public bool IsResolved { get; private set; }
    public string? Resolution { get; private set; }
    public DateTime? ResolvedDate { get; private set; }

    private CloseValidationIssue() { }

    private CloseValidationIssue(string issueDescription, string severity, string? resolution)
    {
        IssueDescription = issueDescription;
        Severity = severity;
        IsResolved = !string.IsNullOrWhiteSpace(resolution);
        Resolution = resolution;
        if (IsResolved) ResolvedDate = DateTime.UtcNow;
    }

    public static CloseValidationIssue Create(string issueDescription, string severity, string? resolution = null)
    {
        return new CloseValidationIssue(issueDescription, severity, resolution);
    }

    public void Resolve(string resolution)
    {
        if (string.IsNullOrWhiteSpace(resolution))
            throw new ArgumentException("Resolution is required", nameof(resolution));

        IsResolved = true;
        Resolution = resolution;
        ResolvedDate = DateTime.UtcNow;
    }
}

