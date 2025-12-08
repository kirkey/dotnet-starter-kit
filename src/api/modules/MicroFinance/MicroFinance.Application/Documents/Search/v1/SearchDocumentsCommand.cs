using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Search.v1;

public class SearchDocumentsCommand : PaginationFilter, IRequest<PagedList<DocumentSummaryResponse>>
{
    public string? DocumentType { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public string? EntityType { get; set; }
    public DefaultIdType? EntityId { get; set; }
    public DateOnly? IssueDateFrom { get; set; }
    public DateOnly? IssueDateTo { get; set; }
    public DateOnly? ExpiryDateFrom { get; set; }
    public DateOnly? ExpiryDateTo { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsRequired { get; set; }
}

public sealed record DocumentSummaryResponse(
    DefaultIdType Id,
    string DocumentType,
    string? Category,
    string Status,
    string EntityType,
    DefaultIdType EntityId,
    string FilePath,
    string? MimeType,
    long FileSizeBytes,
    string? OriginalFileName,
    DateOnly? IssueDate,
    DateOnly? ExpiryDate,
    bool IsVerified,
    bool IsRequired
);
