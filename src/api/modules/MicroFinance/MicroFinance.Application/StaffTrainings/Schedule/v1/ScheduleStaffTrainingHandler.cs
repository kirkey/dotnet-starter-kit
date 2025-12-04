using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Schedule.v1;

/// <summary>
/// Handler for scheduling a new staff training.
/// </summary>
public sealed class ScheduleStaffTrainingHandler(
    ILogger<ScheduleStaffTrainingHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IRepository<StaffTraining> repository)
    : IRequestHandler<ScheduleStaffTrainingCommand, ScheduleStaffTrainingResponse>
{
    public async Task<ScheduleStaffTrainingResponse> Handle(ScheduleStaffTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = StaffTraining.Schedule(
            request.StaffId,
            request.TrainingName,
            request.TrainingType,
            request.DeliveryMethod,
            request.StartDate,
            request.EndDate,
            request.DurationHours,
            request.TrainingCode,
            request.Provider,
            request.Location,
            request.Description,
            request.IsMandatory,
            request.TrainingCost,
            request.PassingScore);

        await repository.AddAsync(training, cancellationToken);
        logger.LogInformation("Staff training '{Name}' scheduled with ID {Id}", training.TrainingName, training.Id);

        return new ScheduleStaffTrainingResponse(training.Id, training.TrainingName, training.StartDate, training.Status);
    }
}
