using FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;

public sealed class SearchSerialNumbersSpec : EntitiesByPaginationFilterSpec<SerialNumber, SerialNumberResponse>
{
    public SearchSerialNumbersSpec(SearchSerialNumbersCommand request) : base(request)
    {
        Query
            .OrderBy(s => s.CreatedOn)
            .ThenBy(s => s.SerialNumberValue);

        if (!string.IsNullOrWhiteSpace(request.SerialNumberValue))
        {
            Query.Where(s => s.SerialNumberValue.Contains(request.SerialNumberValue));
        }

        if (request.ItemId.HasValue)
        {
            Query.Where(s => s.ItemId == request.ItemId.Value);
        }

        if (request.WarehouseId.HasValue)
        {
            Query.Where(s => s.WarehouseId == request.WarehouseId.Value);
        }

        if (request.WarehouseLocationId.HasValue)
        {
            Query.Where(s => s.WarehouseLocationId == request.WarehouseLocationId.Value);
        }

        if (request.BinId.HasValue)
        {
            Query.Where(s => s.BinId == request.BinId.Value);
        }

        if (request.LotNumberId.HasValue)
        {
            Query.Where(s => s.LotNumberId == request.LotNumberId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(s => s.Status == request.Status);
        }

        if (request.ReceiptDateFrom.HasValue)
        {
            Query.Where(s => s.ReceiptDate >= request.ReceiptDateFrom.Value);
        }

        if (request.ReceiptDateTo.HasValue)
        {
            Query.Where(s => s.ReceiptDate <= request.ReceiptDateTo.Value);
        }

        if (request.HasWarranty.HasValue)
        {
            if (request.HasWarranty.Value)
            {
                Query.Where(s => s.WarrantyExpirationDate != null);
            }
            else
            {
                Query.Where(s => s.WarrantyExpirationDate == null);
            }
        }

        if (request.IsWarrantyExpired.HasValue)
        {
            var now = DateTime.UtcNow;
            if (request.IsWarrantyExpired.Value)
            {
                Query.Where(s => s.WarrantyExpirationDate != null && s.WarrantyExpirationDate.Value <= now);
            }
            else
            {
                Query.Where(s => s.WarrantyExpirationDate == null || s.WarrantyExpirationDate.Value > now);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.ExternalReference))
        {
            Query.Where(s => s.ExternalReference != null && s.ExternalReference.Contains(request.ExternalReference));
        }
    }
}
