using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Create.v1;

/// <summary>
/// Command to create a new loan officer target.
/// </summary>
public sealed record CreateLoanOfficerTargetCommand(
    Guid StaffId,
    string TargetType,
    string Period,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TargetValue,
    string? MetricUnit = null,
    string? Description = null,
    decimal? MinimumThreshold = null,
    decimal? StretchTarget = null,
    decimal Weight = 1.0m,
    decimal? IncentiveAmount = null,
    decimal? StretchBonus = null) : IRequest<CreateLoanOfficerTargetResponse>;
