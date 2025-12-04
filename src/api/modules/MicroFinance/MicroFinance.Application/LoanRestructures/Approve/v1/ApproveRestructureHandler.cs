using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;

public sealed class ApproveRestructureHandler(
    [FromKeyedServices("microfinance:loanrestructures")] IRepository<LoanRestructure> repository,
    ILogger<ApproveRestructureHandler> logger)
    : IRequestHandler<ApproveRestructureCommand, ApproveRestructureResponse>
{
    public async Task<ApproveRestructureResponse> Handle(
        ApproveRestructureCommand request,
        CancellationToken cancellationToken)
    {
        var restructure = await repository.FirstOrDefaultAsync(
            new LoanRestructureByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan restructure {request.Id} not found");

        restructure.Approve(request.UserId, request.ApproverName, request.EffectiveDate);
        await repository.UpdateAsync(restructure, cancellationToken);

        logger.LogInformation("Loan restructure approved: {RestructureId}", restructure.Id);

        return new ApproveRestructureResponse(restructure.Id, restructure.Status, request.EffectiveDate);
    }
}
