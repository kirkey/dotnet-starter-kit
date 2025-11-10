using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Search.v1;

/// <summary>
/// Specification for searching meters with filtering and pagination.
/// Projects results to <see cref="MeterResponse"/>.
/// </summary>
public sealed class SearchMetersSpec : EntitiesByPaginationFilterSpec<Meter, MeterResponse>
{
    public SearchMetersSpec(SearchMetersRequest request) : base(request)
    {
        Query
            .OrderBy(m => m.MeterNumber, !request.HasOrderBy())
            .Where(m => m.MeterNumber.Contains(request.MeterNumber!), !string.IsNullOrWhiteSpace(request.MeterNumber))
            .Where(m => m.MeterType == request.MeterType!, !string.IsNullOrWhiteSpace(request.MeterType))
            .Where(m => m.Manufacturer.Contains(request.Manufacturer!), !string.IsNullOrWhiteSpace(request.Manufacturer))
            .Where(m => m.Status == request.Status!, !string.IsNullOrWhiteSpace(request.Status))
            .Where(m => m.MemberId == request.MemberId!.Value, request.MemberId.HasValue)
            .Where(m => m.IsSmartMeter == request.IsSmartMeter!.Value, request.IsSmartMeter.HasValue);
    }
}

