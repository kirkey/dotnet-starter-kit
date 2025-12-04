using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Cancel.v1;

/// <summary>
/// Command to cancel a scheduled training.
/// </summary>
public sealed record CancelStaffTrainingCommand(Guid Id, string? Reason = null) : IRequest<CancelStaffTrainingResponse>;
