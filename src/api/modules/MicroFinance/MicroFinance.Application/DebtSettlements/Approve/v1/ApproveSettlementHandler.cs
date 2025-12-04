using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;

public sealed class ApproveSettlementHandler(
    [FromKeyedServices("microfinance:debtsettlements")] IRepository<DebtSettlement> repository,
    ILogger<ApproveSettlementHandler> logger)
    : IRequestHandler<ApproveSettlementCommand, ApproveSettlementResponse>
{
    public async Task<ApproveSettlementResponse> Handle(
        ApproveSettlementCommand request,
        CancellationToken cancellationToken)
    {
        var settlement = await repository.FirstOrDefaultAsync(
            new DebtSettlementByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Debt settlement {request.Id} not found");

        settlement.Approve(request.ApprovedById);
        await repository.UpdateAsync(settlement, cancellationToken);

        logger.LogInformation("Debt settlement approved: {SettlementId}", settlement.Id);

        return new ApproveSettlementResponse(settlement.Id, settlement.Status, settlement.ApprovedDate!.Value);
    }
}
