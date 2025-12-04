using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Create.v1;

public sealed class CreateDebtSettlementHandler(
    [FromKeyedServices("microfinance:debtsettlements")] IRepository<DebtSettlement> repository,
    ILogger<CreateDebtSettlementHandler> logger)
    : IRequestHandler<CreateDebtSettlementCommand, CreateDebtSettlementResponse>
{
    public async Task<CreateDebtSettlementResponse> Handle(
        CreateDebtSettlementCommand request,
        CancellationToken cancellationToken)
    {
        DebtSettlement settlement;

        if (request.SettlementType == DebtSettlement.TypeInstallment && request.NumberOfInstallments.HasValue)
        {
            settlement = DebtSettlement.CreateInstallment(
                request.ReferenceNumber,
                request.CollectionCaseId,
                request.LoanId,
                request.MemberId,
                request.OriginalOutstanding,
                request.SettlementAmount,
                request.NumberOfInstallments.Value,
                request.DueDate,
                request.Terms,
                request.ProposedById);
        }
        else
        {
            settlement = DebtSettlement.CreateLumpSum(
                request.ReferenceNumber,
                request.CollectionCaseId,
                request.LoanId,
                request.MemberId,
                request.OriginalOutstanding,
                request.SettlementAmount,
                request.DueDate,
                request.Terms,
                request.ProposedById);
        }

        await repository.AddAsync(settlement, cancellationToken);

        logger.LogInformation("Debt settlement created: {SettlementId} for loan {LoanId}",
            settlement.Id, request.LoanId);

        return new CreateDebtSettlementResponse(settlement.Id);
    }
}
