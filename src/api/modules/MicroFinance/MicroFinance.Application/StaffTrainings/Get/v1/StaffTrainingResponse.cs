namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;

/// <summary>
/// Response containing staff training details.
/// </summary>
public sealed record StaffTrainingResponse(
    Guid Id,
    Guid StaffId,
    string? TrainingCode,
    string TrainingName,
    string? Description,
    string TrainingType,
    string DeliveryMethod,
    string? Provider,
    string? Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    int? DurationHours,
    decimal? Score,
    decimal? PassingScore,
    bool CertificateIssued,
    string? CertificationNumber,
    DateOnly? CertificationDate,
    DateOnly? CertificationExpiryDate,
    decimal? TrainingCost,
    bool IsMandatory,
    string Status,
    DateOnly? CompletionDate,
    string? Notes);
