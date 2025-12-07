using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;

/// <summary>
/// Handler to get a staff training by ID.
/// </summary>
public sealed class GetStaffTrainingHandler(
    ILogger<GetStaffTrainingHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IReadRepository<StaffTraining> repository)
    : IRequestHandler<GetStaffTrainingRequest, StaffTrainingResponse>
{
    public async Task<StaffTrainingResponse> Handle(GetStaffTrainingRequest request, CancellationToken cancellationToken)
    {
        var training = await repository.FirstOrDefaultAsync(new StaffTrainingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Staff training {request.Id} not found");

        logger.LogInformation("Retrieved staff training {Id}", training.Id);

        return new StaffTrainingResponse(
            training.Id,
            training.StaffId,
            training.TrainingCode,
            training.TrainingName,
            training.Description,
            training.TrainingType,
            training.DeliveryMethod,
            training.Provider,
            training.Location,
            training.StartDate,
            training.EndDate,
            training.DurationHours,
            training.Score,
            training.PassingScore,
            training.CertificateIssued,
            training.CertificationNumber,
            training.CertificationDate,
            training.CertificationExpiryDate,
            training.TrainingCost,
            training.IsMandatory,
            training.Status,
            training.CompletionDate,
            training.Notes);
    }
}
