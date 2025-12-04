namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.IssueCertificate.v1;

/// <summary>
/// Response from issuing a training certificate.
/// </summary>
public sealed record IssueCertificateResponse(
    Guid Id,
    string CertificationNumber,
    DateOnly CertificationDate,
    DateOnly? ExpiryDate);
