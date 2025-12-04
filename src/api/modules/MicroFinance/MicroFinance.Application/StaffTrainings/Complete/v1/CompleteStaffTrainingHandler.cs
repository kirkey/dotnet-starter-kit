using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Complete.v1;

/// <summary>
/// Handler to complete a staff training with results.
/// </summary>
public sealed class CompleteStaffTrainingHandler(
    ILogger<CompleteStaffTrainingHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IRepository<StaffTraining> repository)
    : IRequestHandler<CompleteStaffTrainingCommand, CompleteStaffTrainingResponse>
{
    public async Task<CompleteStaffTrainingResponse> Handle(CompleteStaffTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = await repository.FirstOrDefaultAsync(new StaffTrainingByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Staff training {request.Id} not found");

        training.Complete(request.Score, request.CompletionDate);
        await repository.UpdateAsync(training, cancellationToken);

        logger.LogInformation("Staff training {Id} completed with score {Score}, status {Status}",
            training.Id, request.Score, training.Status);

        return new CompleteStaffTrainingResponse(training.Id, training.Status, training.Score, training.CompletionDate);
    }
}
