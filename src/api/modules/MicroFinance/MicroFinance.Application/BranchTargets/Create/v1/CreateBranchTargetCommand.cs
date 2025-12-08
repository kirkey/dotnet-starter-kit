using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Create.v1;

/// <summary>
/// Command to create a new branch target.
/// </summary>
public sealed record CreateBranchTargetCommand(
    DefaultIdType BranchId,
    string TargetType,
    string Period,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TargetValue,
    string? MetricUnit = null,
    string? Description = null,
    decimal? MinimumThreshold = null,
    decimal? StretchTarget = null,
    decimal Weight = 1.0m) : IRequest<CreateBranchTargetResponse>;
