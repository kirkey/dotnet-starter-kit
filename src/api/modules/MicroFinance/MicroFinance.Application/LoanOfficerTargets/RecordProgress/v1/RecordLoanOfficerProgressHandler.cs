using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.RecordProgress.v1;

/// <summary>
/// Handler to record progress towards a loan officer target.
/// </summary>
public sealed class RecordLoanOfficerProgressHandler(
    ILogger<RecordLoanOfficerProgressHandler> logger,
    [FromKeyedServices("microfinance:loanofficertargets")] IRepository<LoanOfficerTarget> repository)
    : IRequestHandler<RecordLoanOfficerProgressCommand, RecordLoanOfficerProgressResponse>
{
    public async Task<RecordLoanOfficerProgressResponse> Handle(RecordLoanOfficerProgressCommand request, CancellationToken cancellationToken)
    {
        var target = await repository.FirstOrDefaultAsync(new LoanOfficerTargetByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Loan officer target {request.Id} not found");

        target.RecordProgress(request.AchievedValue);
        await repository.UpdateAsync(target, cancellationToken);

        var earnedIncentive = target.CalculateEarnedIncentive();

        logger.LogInformation("Loan officer target {Id} progress recorded: {Achieved}/{Target} ({Percentage}%), Incentive: {Incentive}",
            target.Id, target.AchievedValue, target.TargetValue, target.AchievementPercentage, earnedIncentive);

        return new RecordLoanOfficerProgressResponse(
            target.Id,
            target.AchievedValue,
            target.AchievementPercentage,
            target.Status,
            earnedIncentive);
    }
}
