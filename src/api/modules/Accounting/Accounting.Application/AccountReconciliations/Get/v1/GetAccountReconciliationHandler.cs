using Accounting.Application.AccountReconciliations.Responses;

namespace Accounting.Application.AccountReconciliations.Get.v1;

/// <summary>
/// Handler for getting account reconciliation by ID.
/// </summary>
public sealed class GetAccountReconciliationHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IReadRepository<AccountReconciliation> repository,
    ILogger<GetAccountReconciliationHandler> logger)
    : IRequestHandler<GetAccountReconciliationRequest, AccountReconciliationResponse>
{
    public async Task<AccountReconciliationResponse> Handle(GetAccountReconciliationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reconciliation = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (reconciliation == null)
            throw new AccountReconciliationNotFoundException(request.Id);

        logger.LogInformation("Retrieved account reconciliation {Id}", request.Id);

        return new AccountReconciliationResponse
        {
            Id = reconciliation.Id,
            GeneralLedgerAccountId = reconciliation.GeneralLedgerAccountId,
            AccountingPeriodId = reconciliation.AccountingPeriodId,
            GlBalance = reconciliation.GlBalance,
            SubsidiaryLedgerBalance = reconciliation.SubsidiaryLedgerBalance,
            Variance = reconciliation.Variance,
            ReconciliationStatus = reconciliation.ReconciliationStatus,
            ReconciliationDate = reconciliation.ReconciliationDate,
            VarianceExplanation = reconciliation.VarianceExplanation,
            SubsidiaryLedgerSource = reconciliation.SubsidiaryLedgerSource,
            LineItemCount = reconciliation.LineItemCount,
            AdjustingEntriesRecorded = reconciliation.AdjustingEntriesRecorded,
            Status = reconciliation.Status,
            CreatedOn = reconciliation.CreatedOn,
            ApprovedBy = reconciliation.ApprovedBy,
            ApproverName = reconciliation.ApproverName,
            ApprovedOn = reconciliation.ApprovedOn,
            Remarks = reconciliation.Remarks,
            NotesCount = reconciliation.ReconciliationNotes.Count
        };
    }
}

