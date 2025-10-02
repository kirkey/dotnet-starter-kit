using Accounting.Application.BankReconciliations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Get.v1;

public sealed class GetBankReconciliationHandler(
    IReadRepository<BankReconciliation> repository)
    : IRequestHandler<GetBankReconciliationRequest, BankReconciliationResponse>
{
    public async Task<BankReconciliationResponse> Handle(GetBankReconciliationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reconciliation = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(request.Id);

        return new BankReconciliationResponse
        {
            Id = reconciliation.Id,
            BankAccountId = reconciliation.BankAccountId,
            ReconciliationDate = reconciliation.ReconciliationDate,
            StatementBalance = reconciliation.StatementBalance,
            BookBalance = reconciliation.BookBalance,
            AdjustedBalance = reconciliation.AdjustedBalance,
            OutstandingChecksTotal = reconciliation.OutstandingChecksTotal,
            DepositsInTransitTotal = reconciliation.DepositsInTransitTotal,
            BankErrors = reconciliation.BankErrors,
            BookErrors = reconciliation.BookErrors,
            Status = reconciliation.Status.ToString(),
            IsReconciled = reconciliation.IsReconciled,
            ReconciledDate = reconciliation.ReconciledDate,
            ReconciledBy = reconciliation.ReconciledBy,
            ApprovedBy = reconciliation.ApprovedBy,
            ApprovedDate = reconciliation.ApprovedDate,
            StatementNumber = reconciliation.StatementNumber,
            Description = reconciliation.Description,
            Notes = reconciliation.Notes,
            CreatedOn = reconciliation.CreatedOn,
            CreatedBy = reconciliation.CreatedBy,
            LastModifiedOn = reconciliation.LastModifiedOn,
            LastModifiedBy = reconciliation.LastModifiedBy
        };
    }
}
