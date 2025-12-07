using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;

/// <summary>
/// Handler to get a loan officer target by ID.
/// </summary>
public sealed class GetLoanOfficerTargetHandler(
    ILogger<GetLoanOfficerTargetHandler> logger,
    [FromKeyedServices("microfinance:loanofficertargets")] IReadRepository<LoanOfficerTarget> repository)
    : IRequestHandler<GetLoanOfficerTargetRequest, LoanOfficerTargetResponse>
{
    public async Task<LoanOfficerTargetResponse> Handle(GetLoanOfficerTargetRequest request, CancellationToken cancellationToken)
    {
        var target = await repository.FirstOrDefaultAsync(new LoanOfficerTargetByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan officer target {request.Id} not found");

        logger.LogInformation("Retrieved loan officer target {Id}", target.Id);

        return new LoanOfficerTargetResponse(
            target.Id,
            target.StaffId,
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
            target.IncentiveAmount,
            target.StretchBonus,
            target.CalculateEarnedIncentive(),
            target.Notes);
    }
}
