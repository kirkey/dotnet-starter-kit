using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Create.v1;

/// <summary>
/// Handler for creating a new branch target.
/// </summary>
public sealed class CreateBranchTargetHandler(
    ILogger<CreateBranchTargetHandler> logger,
    [FromKeyedServices("microfinance:branchtargets")] IRepository<BranchTarget> repository)
    : IRequestHandler<CreateBranchTargetCommand, CreateBranchTargetResponse>
{
    public async Task<CreateBranchTargetResponse> Handle(CreateBranchTargetCommand request, CancellationToken cancellationToken)
    {
        var target = BranchTarget.Create(
            request.BranchId,
            request.TargetType,
            request.Period,
            request.PeriodStart,
            request.PeriodEnd,
            request.TargetValue,
            request.MetricUnit,
            request.Description,
            request.MinimumThreshold,
            request.StretchTarget,
            request.Weight);

        await repository.AddAsync(target, cancellationToken);
        logger.LogInformation("Branch target {TargetType} created with ID {Id}", target.TargetType, target.Id);

        return new CreateBranchTargetResponse(target.Id, target.TargetType, target.TargetValue, target.Status);
    }
}
