using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Search.v1;

public class SearchStaffTrainingsCommand : PaginationFilter, IRequest<PagedList<StaffTrainingSummaryResponse>>
{
    public DefaultIdType? StaffId { get; set; }
    public string? TrainingCode { get; set; }
    public string? TrainingType { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? Status { get; set; }
    public string? Provider { get; set; }
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
    public bool? IsMandatory { get; set; }
    public bool? CertificateIssued { get; set; }
}

public sealed record StaffTrainingSummaryResponse(
    DefaultIdType Id,
    DefaultIdType StaffId,
    string? TrainingCode,
    string TrainingName,
    string TrainingType,
    string DeliveryMethod,
    string? Provider,
    string? Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    int? DurationHours,
    decimal? Score,
    bool CertificateIssued,
    string? CertificationNumber,
    DateOnly? CertificationExpiryDate,
    bool IsMandatory,
    string Status,
    DateOnly? CompletionDate
);
