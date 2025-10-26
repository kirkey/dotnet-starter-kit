namespace FSH.Starter.WebApi.Store.Application.PickLists;

/// <summary>
/// Specification to retrieve a pick list by ID with its items and related entities.
/// </summary>
public sealed class GetPickListByIdSpec : Specification<PickList>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPickListByIdSpec"/> class.
    /// </summary>
    /// <param name="pickListId">The pick list ID to retrieve.</param>
    public GetPickListByIdSpec(DefaultIdType pickListId)
    {
        Query
            .Where(x => x.Id == pickListId)
            .Include(x => x.Warehouse)
            .Include(x => x.Items)
                .ThenInclude(item => item.Item)
            .Include(x => x.Items)
                .ThenInclude(item => item.Bin);
    }
}
