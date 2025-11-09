using FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

public class SearchInventoryReservationsSpec : EntitiesByPaginationFilterSpec<InventoryReservation, InventoryReservationResponse>
{
    public SearchInventoryReservationsSpec(SearchInventoryReservationsCommand request)
        : base(request)
    {
        Query
            .OrderByDescending(r => r.ReservationDate)
            .ThenBy(r => r.ReservationNumber);

        if (!string.IsNullOrWhiteSpace(request.ReservationNumber))
        {
            Query.Where(r => r.ReservationNumber.Contains(request.ReservationNumber));
        }

        if (request.ItemId.HasValue)
        {
            Query.Where(r => r.ItemId == request.ItemId.Value);
        }

        if (request.WarehouseId.HasValue)
        {
            Query.Where(r => r.WarehouseId == request.WarehouseId.Value);
        }

        if (request.WarehouseLocationId.HasValue)
        {
            Query.Where(r => r.WarehouseLocationId == request.WarehouseLocationId.Value);
        }

        if (request.BinId.HasValue)
        {
            Query.Where(r => r.BinId == request.BinId.Value);
        }

        if (request.LotNumberId.HasValue)
        {
            Query.Where(r => r.LotNumberId == request.LotNumberId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.ReservationType))
        {
            Query.Where(r => r.ReservationType == request.ReservationType);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(r => r.Status == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            Query.Where(r => r.ReferenceNumber != null && r.ReferenceNumber.Contains(request.ReferenceNumber));
        }

        if (request.ReservationDateFrom.HasValue)
        {
            Query.Where(r => r.ReservationDate >= request.ReservationDateFrom.Value);
        }

        if (request.ReservationDateTo.HasValue)
        {
            Query.Where(r => r.ReservationDate <= request.ReservationDateTo.Value);
        }

        if (request.ExpirationDateFrom.HasValue)
        {
            Query.Where(r => r.ExpirationDate != null && r.ExpirationDate.Value >= request.ExpirationDateFrom.Value);
        }

        if (request.ExpirationDateTo.HasValue)
        {
            Query.Where(r => r.ExpirationDate != null && r.ExpirationDate.Value <= request.ExpirationDateTo.Value);
        }

        if (request.IsExpired.HasValue)
        {
            var now = DateTime.UtcNow;
            if (request.IsExpired.Value)
            {
                Query.Where(r => r.ExpirationDate != null && r.ExpirationDate.Value <= now && r.Status == "Active");
            }
            else
            {
                Query.Where(r => r.ExpirationDate == null || r.ExpirationDate.Value > now || r.Status != "Active");
            }
        }

        if (request.IsActive.HasValue)
        {
            var now = DateTime.UtcNow;
            if (request.IsActive.Value)
            {
                Query.Where(r => r.Status == "Active" && (r.ExpirationDate == null || r.ExpirationDate.Value > now));
            }
            else
            {
                Query.Where(r => r.Status != "Active" || (r.ExpirationDate != null && r.ExpirationDate.Value <= now));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.ReservedBy))
        {
            Query.Where(r => r.ReservedBy != null && r.ReservedBy.Contains(request.ReservedBy));
        }
    }
}
