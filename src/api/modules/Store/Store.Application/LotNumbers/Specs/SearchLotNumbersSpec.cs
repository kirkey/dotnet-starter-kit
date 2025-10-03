using FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;
using LotNumberResponse = FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1.LotNumberResponse;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Specs;

/// <summary>
/// Specification for searching lot numbers with filters and pagination.
/// </summary>
public sealed class SearchLotNumbersSpec : EntitiesByPaginationFilterSpec<LotNumber, LotNumberResponse>
{
    public SearchLotNumbersSpec(SearchLotNumbersCommand request)
        : base(request)
    {
        var now = DateTime.UtcNow;

        Query
            .Where(l => l.ItemId == request.ItemId, request.ItemId.HasValue)
            .Where(l => l.SupplierId == request.SupplierId, request.SupplierId.HasValue)
            .Where(l => l.LotCode.Contains(request.LotCode!), !string.IsNullOrWhiteSpace(request.LotCode))
            .Where(l => l.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(l => l.ExpirationDate >= request.ExpirationDateFrom, request.ExpirationDateFrom.HasValue)
            .Where(l => l.ExpirationDate <= request.ExpirationDateTo, request.ExpirationDateTo.HasValue)
            .Where(l => l.ExpirationDate.HasValue && l.ExpirationDate.Value <= now.AddDays(request.ExpiringWithinDays!.Value), request.ExpiringWithinDays.HasValue)
            .Where(l => l.ExpirationDate.HasValue && l.ExpirationDate.Value <= now, request.IsExpired == true)
            .Where(l => !l.ExpirationDate.HasValue || l.ExpirationDate.Value > now, request.IsExpired == false)
            .Where(l => l.QuantityRemaining >= request.MinQuantityRemaining, request.MinQuantityRemaining.HasValue);

        Query.Select(l => new LotNumberResponse(
            l.Id,
            l.LotCode,
            l.ItemId,
            l.SupplierId,
            l.ManufactureDate,
            l.ExpirationDate,
            l.ReceiptDate,
            l.QuantityReceived,
            l.QuantityRemaining,
            l.Status,
            l.QualityNotes,
            l.CreatedOn,
            l.CreatedBy));
    }
}
