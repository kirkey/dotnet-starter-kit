namespace Accounting.Application.FiscalPeriodCloses.Queries;

/// <summary>
/// Fiscal period close data transfer object for list views.
/// </summary>
public record FiscalPeriodCloseDto
{
    public DefaultIdType Id { get; init; }
    public string CloseNumber { get; init; } = string.Empty;
    public DefaultIdType PeriodId { get; init; }
    public string CloseType { get; init; } = string.Empty;
    public DateTime PeriodStartDate { get; init; }
    public DateTime PeriodEndDate { get; init; }
    public DateTime CloseInitiatedDate { get; init; }
    public string InitiatedBy { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsComplete { get; init; }
    public DateTime? CompletedDate { get; init; }
    public int TasksCompleted { get; init; }
    public int TasksRemaining { get; init; }
    public decimal CompletionPercentage { get; init; }
    public bool RequiredTasksComplete { get; init; }
}

/// <summary>
/// Fiscal period close data transfer object for detail view with all properties and tasks.
/// </summary>
public record FiscalPeriodCloseDetailsDto : FiscalPeriodCloseDto
{
    public string? CompletedBy { get; init; }
    public bool TrialBalanceGenerated { get; init; }
    public bool TrialBalanceBalanced { get; init; }
    public bool AllJournalsPosted { get; init; }
    public bool BankReconciliationsComplete { get; init; }
    public bool APReconciliationComplete { get; init; }
    public bool ARReconciliationComplete { get; init; }
    public bool InventoryReconciliationComplete { get; init; }
    public bool FixedAssetDepreciationPosted { get; init; }
    public bool PrepaidExpensesAmortized { get; init; }
    public bool AccrualsPosted { get; init; }
    public bool IntercompanyReconciled { get; init; }
    public bool NetIncomeTransferred { get; init; }
    public DefaultIdType? TrialBalanceId { get; init; }
    public decimal? FinalNetIncome { get; init; }
    public string? ReopenReason { get; init; }
    public DateTime? ReopenedDate { get; init; }
    public string? ReopenedBy { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public List<CloseTaskItemDto> Tasks { get; init; } = new();
    public List<CloseValidationIssueDto> ValidationIssues { get; init; } = new();
    public bool HasUnresolvedCriticalIssues { get; init; }
}

/// <summary>
/// Close task item data transfer object.
/// </summary>
public record CloseTaskItemDto
{
    public string TaskName { get; init; } = string.Empty;
    public bool IsRequired { get; init; }
    public bool IsComplete { get; init; }
    public DateTime? CompletedDate { get; init; }
}

/// <summary>
/// Close validation issue data transfer object.
/// </summary>
public record CloseValidationIssueDto
{
    public string IssueDescription { get; init; } = string.Empty;
    public string Severity { get; init; } = string.Empty;
    public bool IsResolved { get; init; }
    public string? Resolution { get; init; }
    public DateTime? ResolvedDate { get; init; }
}

