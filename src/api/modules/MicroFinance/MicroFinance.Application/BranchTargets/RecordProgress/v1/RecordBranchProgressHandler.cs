using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.RecordProgress.v1;

/// <summary>
/// Handler to record progress towards a branch target.
/// </summary>
public sealed class RecordBranchProgressHandler(
    ILogger<RecordBranchProgressHandler> logger,
    [FromKeyedServices("microfinance:branchtargets")] IRepository<BranchTarget> repository)
    : IRequestHandler<RecordBranchProgressCommand, RecordBranchProgressResponse>
{
    public async Task<RecordBranchProgressResponse> Handle(RecordBranchProgressCommand request, CancellationToken cancellationToken)
    {
        var target = await repository.FirstOrDefaultAsync(new BranchTargetByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Branch target {request.Id} not found");

        target.RecordProgress(request.AchievedValue);
        await repository.UpdateAsync(target, cancellationToken);

        logger.LogInformation("Branch target {Id} progress recorded: {Achieved}/{Target} ({Percentage}%)",
            target.Id, target.AchievedValue, target.TargetValue, target.AchievementPercentage);

        return new RecordBranchProgressResponse(
            target.Id,
            target.AchievedValue,
            target.AchievementPercentage,
            target.Status);
    }
}
