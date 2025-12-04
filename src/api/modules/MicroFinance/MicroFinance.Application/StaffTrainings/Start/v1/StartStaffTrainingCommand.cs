using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Start.v1;

/// <summary>
/// Command to start a scheduled training.
/// </summary>
public sealed record StartStaffTrainingCommand(Guid Id) : IRequest<StartStaffTrainingResponse>;
