using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Complete.v1;

/// <summary>
/// Command to complete a training with results.
/// </summary>
public sealed record CompleteStaffTrainingCommand(
    DefaultIdType Id,
    decimal? Score = null,
    DateOnly? CompletionDate = null) : IRequest<CompleteStaffTrainingResponse>;
