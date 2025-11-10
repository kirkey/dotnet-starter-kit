using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Search.v1;

/// <summary>
/// Request to search for meters with optional filters and pagination.
/// </summary>
public sealed class SearchMetersRequest : PaginationFilter, IRequest<PagedList<MeterResponse>>
{
    public string? MeterNumber { get; init; }
    public string? MeterType { get; init; }
    public string? Manufacturer { get; init; }
    public string? Status { get; init; }
    public DefaultIdType? MemberId { get; init; }
    public bool? IsSmartMeter { get; init; }
}

