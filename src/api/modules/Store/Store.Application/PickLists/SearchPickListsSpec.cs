using FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PickLists;

/// <summary>
/// Specification for searching and filtering pick lists with pagination.
/// </summary>
public sealed class SearchPickListsSpec : EntitiesByPaginationFilterSpec<PickList>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPickListsSpec"/> class.
    /// </summary>
    /// <param name="request">The search pick lists command with filter criteria.</param>
    public SearchPickListsSpec(SearchPickListsCommand request) : base(request)
    {
        Query
            .Include(x => x.Warehouse)
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
    }
}
