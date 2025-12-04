using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;

/// <summary>
/// Handler to get a branch target by ID.
/// </summary>
public sealed class GetBranchTargetHandler(
    ILogger<GetBranchTargetHandler> logger,
    [FromKeyedServices("microfinance:branchtargets")] IReadRepository<BranchTarget> repository)
    : IRequestHandler<GetBranchTargetRequest, BranchTargetResponse>
{
    public async Task<BranchTargetResponse> Handle(GetBranchTargetRequest request, CancellationToken cancellationToken)
    {
        var target = await repository.FirstOrDefaultAsync(new BranchTargetByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Branch target {request.Id} not found");

        logger.LogInformation("Retrieved branch target {Id}", target.Id);

        return new BranchTargetResponse(
            target.Id,
            target.BranchId,
            target.TargetType,
            target.Description,
            target.Period,
            target.PeriodStart,
            target.PeriodEnd,
            target.TargetValue,
            target.MetricUnit,
            target.AchievedValue,
            target.AchievementPercentage,
            target.Status,
            target.MinimumThreshold,
            target.StretchTarget,
            target.Weight,
            target.Notes);
    }
}
