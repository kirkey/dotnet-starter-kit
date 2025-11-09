using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Search.v1;

/// <summary>
/// Request to search for write-offs with optional filters and pagination support.
/// </summary>
public class SearchWriteOffsRequest : PaginationFilter, IRequest<PagedList<WriteOffResponse>>
{
    public string? ReferenceNumber { get; set; }
    public DefaultIdType? CustomerId { get; set; }
    public string? WriteOffType { get; set; }
    public string? Status { get; set; }
    public bool? IsRecovered { get; set; }
}

