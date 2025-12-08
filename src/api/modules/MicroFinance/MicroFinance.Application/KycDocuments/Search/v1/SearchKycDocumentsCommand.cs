using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Search.v1;

public class SearchKycDocumentsCommand : PaginationFilter, IRequest<PagedList<KycDocumentSummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Status { get; set; }
    public DateOnly? ExpiryDateFrom { get; set; }
    public DateOnly? ExpiryDateTo { get; set; }
    public bool? IsVerified { get; set; }
}

public sealed record KycDocumentSummaryResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    string DocumentType,
    string? DocumentNumber,
    string Status,
    DateOnly? ExpiryDate,
    string? FileName,
    DateTimeOffset CreatedOn);
