using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Create.v1;

/// <summary>
/// Handler for creating a new loan officer target.
/// </summary>
public sealed class CreateLoanOfficerTargetHandler(
    ILogger<CreateLoanOfficerTargetHandler> logger,
    [FromKeyedServices("microfinance:loanofficertargets")] IRepository<LoanOfficerTarget> repository)
    : IRequestHandler<CreateLoanOfficerTargetCommand, CreateLoanOfficerTargetResponse>
{
    public async Task<CreateLoanOfficerTargetResponse> Handle(CreateLoanOfficerTargetCommand request, CancellationToken cancellationToken)
    {
        var target = LoanOfficerTarget.Create(
            request.StaffId,
            request.TargetType,
            request.Period,
            request.PeriodStart,
            request.PeriodEnd,
            request.TargetValue,
            request.MetricUnit,
            request.Description,
            request.MinimumThreshold,
            request.StretchTarget,
            request.Weight,
            request.IncentiveAmount,
            request.StretchBonus);

        await repository.AddAsync(target, cancellationToken);
        logger.LogInformation("Loan officer target {TargetType} created with ID {Id}", target.TargetType, target.Id);

        return new CreateLoanOfficerTargetResponse(target.Id, target.TargetType, target.TargetValue, target.Status);
    }
}
