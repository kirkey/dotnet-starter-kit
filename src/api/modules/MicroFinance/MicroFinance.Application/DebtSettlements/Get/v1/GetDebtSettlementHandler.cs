using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;

public sealed class GetDebtSettlementHandler(
    [FromKeyedServices("microfinance:debtsettlements")] IReadRepository<DebtSettlement> repository)
    : IRequestHandler<GetDebtSettlementRequest, DebtSettlementResponse>
{
    public async Task<DebtSettlementResponse> Handle(
        GetDebtSettlementRequest request,
        CancellationToken cancellationToken)
    {
        var settlement = await repository.FirstOrDefaultAsync(
            new DebtSettlementByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Debt settlement {request.Id} not found");

        return new DebtSettlementResponse(
            settlement.Id,
            settlement.ReferenceNumber,
            settlement.CollectionCaseId,
            settlement.LoanId,
            settlement.MemberId,
            settlement.SettlementType,
            settlement.Status,
            settlement.OriginalOutstanding,
            settlement.SettlementAmount,
            settlement.DiscountAmount,
            settlement.DiscountPercentage,
            settlement.AmountPaid,
            settlement.RemainingBalance,
            settlement.NumberOfInstallments,
            settlement.InstallmentAmount,
            settlement.ProposedDate,
            settlement.ApprovedDate,
            settlement.DueDate,
            settlement.CompletedDate,
            settlement.Terms,
            settlement.Justification,
            settlement.ProposedById,
            settlement.ApprovedById);
    }
}
