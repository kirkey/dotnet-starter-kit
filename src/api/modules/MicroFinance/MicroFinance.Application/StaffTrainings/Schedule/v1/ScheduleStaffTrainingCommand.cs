using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Schedule.v1;

/// <summary>
/// Command to schedule a new training for a staff member.
/// </summary>
public sealed record ScheduleStaffTrainingCommand(
    DefaultIdType StaffId,
    string TrainingName,
    string TrainingType,
    string DeliveryMethod,
    DateOnly StartDate,
    DateOnly? EndDate = null,
    int? DurationHours = null,
    string? TrainingCode = null,
    string? Provider = null,
    string? Location = null,
    string? Description = null,
    bool IsMandatory = false,
    decimal? TrainingCost = null,
    decimal? PassingScore = null) : IRequest<ScheduleStaffTrainingResponse>;
