using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.IssueCertificate.v1;

/// <summary>
/// Handler to issue a certificate for completed training.
/// </summary>
public sealed class IssueCertificateHandler(
    ILogger<IssueCertificateHandler> logger,
    [FromKeyedServices("microfinance:stafftrainings")] IRepository<StaffTraining> repository)
    : IRequestHandler<IssueCertificateCommand, IssueCertificateResponse>
{
    public async Task<IssueCertificateResponse> Handle(IssueCertificateCommand request, CancellationToken cancellationToken)
    {
        var training = await repository.FirstOrDefaultAsync(new StaffTrainingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Staff training {request.Id} not found");

        training.IssueCertificate(request.CertificationNumber, request.CertificationDate, request.ExpiryDate);
        await repository.UpdateAsync(training, cancellationToken);

        logger.LogInformation("Certificate {Number} issued for training {Id}", request.CertificationNumber, training.Id);

        return new IssueCertificateResponse(
            training.Id,
            training.CertificationNumber!,
            training.CertificationDate!.Value,
            training.CertificationExpiryDate);
    }
}
