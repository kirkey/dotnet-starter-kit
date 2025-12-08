using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.IssueCertificate.v1;

/// <summary>
/// Command to issue a certificate for completed training.
/// </summary>
public sealed record IssueCertificateCommand(
    DefaultIdType Id,
    string CertificationNumber,
    DateOnly? CertificationDate = null,
    DateOnly? ExpiryDate = null) : IRequest<IssueCertificateResponse>;
