using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Cancel.v1;

/// <summary>
/// Handler to cancel a scheduled staff training.
/// </summary>
public sealed class CancelStaffTrainingHandler(
    ILogger<CancelStaffTrainingHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IRepository<StaffTraining> repository)
    : IRequestHandler<CancelStaffTrainingCommand, CancelStaffTrainingResponse>
{
    public async Task<CancelStaffTrainingResponse> Handle(CancelStaffTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = await repository.FirstOrDefaultAsync(new StaffTrainingByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Staff training {request.Id} not found");

        training.Cancel(request.Reason);
        await repository.UpdateAsync(training, cancellationToken);

        logger.LogInformation("Staff training {Id} cancelled. Reason: {Reason}", training.Id, request.Reason);

        return new CancelStaffTrainingResponse(training.Id, training.Status);
    }
}
