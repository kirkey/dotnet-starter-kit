using FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PickLists;

public sealed class SearchPickListsSpec : EntitiesByPaginationFilterSpec<PickList, PickListResponse>
{
    public SearchPickListsSpec(SearchPickListsCommand request) : base(request)
    {
        Query
            .OrderByDescending(x => x.Priority)
            .ThenByDescending(x => x.CreatedOn)
            .ThenBy(x => x.PickListNumber);

        if (!string.IsNullOrWhiteSpace(request.PickListNumber))
        {
            Query.Where(x => x.PickListNumber.Contains(request.PickListNumber));
        }

        if (request.WarehouseId.HasValue)
        {
            Query.Where(x => x.WarehouseId == request.WarehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(x => x.Status == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.PickingType))
        {
            Query.Where(x => x.PickingType == request.PickingType);
        }

        if (!string.IsNullOrWhiteSpace(request.AssignedTo))
        {
            Query.Where(x => x.AssignedTo == request.AssignedTo);
        }

        if (request.StartDateFrom.HasValue)
        {
            Query.Where(x => x.StartDate >= request.StartDateFrom.Value);
        }

        if (request.StartDateTo.HasValue)
        {
            Query.Where(x => x.StartDate <= request.StartDateTo.Value);
        }

        if (request.CompletedDateFrom.HasValue)
        {
            Query.Where(x => x.CompletedDate >= request.CompletedDateFrom.Value);
        }

        if (request.CompletedDateTo.HasValue)
        {
            Query.Where(x => x.CompletedDate <= request.CompletedDateTo.Value);
        }

        if (request.MinPriority.HasValue)
        {
            Query.Where(x => x.Priority >= request.MinPriority.Value);
        }

        if (request.MaxPriority.HasValue)
        {
            Query.Where(x => x.Priority <= request.MaxPriority.Value);
        }

        Query.Select(x => new PickListResponse
        {
            Id = x.Id,
            PickListNumber = x.PickListNumber,
            WarehouseId = x.WarehouseId,
            Status = x.Status,
            PickingType = x.PickingType,
            Priority = x.Priority,
            AssignedTo = x.AssignedTo,
            StartDate = x.StartDate,
            CompletedDate = x.CompletedDate,
            ReferenceNumber = x.ReferenceNumber,
            TotalLines = x.TotalLines,
            CompletedLines = x.CompletedLines
        });
    }
}
