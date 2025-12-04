using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Start.v1;

/// <summary>
/// Handler to start a scheduled staff training.
/// </summary>
public sealed class StartStaffTrainingHandler(
    ILogger<StartStaffTrainingHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IRepository<StaffTraining> repository)
    : IRequestHandler<StartStaffTrainingCommand, StartStaffTrainingResponse>
{
    public async Task<StartStaffTrainingResponse> Handle(StartStaffTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = await repository.FirstOrDefaultAsync(new StaffTrainingByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Staff training {request.Id} not found");

        training.Start();
        await repository.UpdateAsync(training, cancellationToken);

        logger.LogInformation("Staff training {Id} started", training.Id);

        return new StartStaffTrainingResponse(training.Id, training.Status);
    }
}
