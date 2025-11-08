using Accounting.Application.FiscalPeriodCloses.Queries;

namespace Accounting.Application.FiscalPeriodCloses.Get;

/// <summary>
/// Handler for retrieving a fiscal period close by its ID with complete details.
/// </summary>
public sealed class GetFiscalPeriodCloseHandler(
    ILogger<GetFiscalPeriodCloseHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<FiscalPeriodClose> repository)
    : IRequestHandler<GetFiscalPeriodCloseRequest, FiscalPeriodCloseDetailsDto>
{
    public async Task<FiscalPeriodCloseDetailsDto> Handle(GetFiscalPeriodCloseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var close = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (close == null)
        {
            throw new NotFoundException($"Fiscal period close with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved fiscal period close {FiscalPeriodCloseId}", close.Id);

        return new FiscalPeriodCloseDetailsDto
        {
            Id = close.Id,
            CloseNumber = close.CloseNumber,
            PeriodId = close.PeriodId,
            CloseType = close.CloseType,
            PeriodStartDate = close.PeriodStartDate,
            PeriodEndDate = close.PeriodEndDate,
            CloseInitiatedDate = close.CloseInitiatedDate,
            InitiatedBy = close.InitiatedBy,
            Status = close.Status,
            IsComplete = close.IsComplete,
            CompletedDate = close.CompletedDate,
            CompletedBy = close.CompletedBy,
            TasksCompleted = close.TasksCompleted,
            TasksRemaining = close.TasksRemaining,
            CompletionPercentage = close.TasksCompleted + close.TasksRemaining > 0 
                ? (decimal)close.TasksCompleted / (close.TasksCompleted + close.TasksRemaining) * 100 
                : 0,
            RequiredTasksComplete = close.RequiredTasksComplete,
            
            // Validation status
            TrialBalanceGenerated = close.TrialBalanceGenerated,
            TrialBalanceBalanced = close.TrialBalanceBalanced,
            AllJournalsPosted = close.AllJournalsPosted,
            BankReconciliationsComplete = close.BankReconciliationsComplete,
            APReconciliationComplete = close.ApReconciliationComplete,
            ARReconciliationComplete = close.ArReconciliationComplete,
            InventoryReconciliationComplete = close.InventoryReconciliationComplete,
            FixedAssetDepreciationPosted = close.FixedAssetDepreciationPosted,
            PrepaidExpensesAmortized = close.PrepaidExpensesAmortized,
            AccrualsPosted = close.AccrualsPosted,
            IntercompanyReconciled = close.IntercompanyReconciled,
            NetIncomeTransferred = close.NetIncomeTransferred,
            
            // Year-end specific
            TrialBalanceId = close.TrialBalanceId,
            FinalNetIncome = close.FinalNetIncome,
            
            // Reopen tracking
            ReopenReason = close.ReopenReason,
            ReopenedDate = close.ReopenedDate,
            ReopenedBy = close.ReopenedBy,
            
            // Additional info
            Description = close.Description,
            Notes = close.Notes,
            
            // Tasks and validation issues
            Tasks = close.Tasks.Select(t => new CloseTaskItemDto
            {
                TaskName = t.TaskName,
                IsRequired = t.IsRequired,
                IsComplete = t.IsComplete,
                CompletedDate = t.CompletedDate
            }).ToList(),
            
            ValidationIssues = close.ValidationIssues.Select(v => new CloseValidationIssueDto
            {
                IssueDescription = v.IssueDescription,
                Severity = v.Severity,
                IsResolved = v.IsResolved,
                Resolution = v.Resolution,
                ResolvedDate = v.ResolvedDate
            }).ToList(),
            
            HasUnresolvedCriticalIssues = close.ValidationIssues.Any(v => 
                v.Severity == "Critical" && !v.IsResolved)
        };
    }
}



