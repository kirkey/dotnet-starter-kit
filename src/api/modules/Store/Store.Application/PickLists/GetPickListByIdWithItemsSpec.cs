namespace FSH.Starter.WebApi.Store.Application.PickLists;

/// <summary>
/// Specification to retrieve a pick list by ID with its items collection only (no nested navigation properties).
/// This spec is used for write operations to avoid EF Core tracking conflicts.
/// </summary>
public sealed class GetPickListByIdWithItemsSpec : Specification<PickList>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPickListByIdWithItemsSpec"/> class.
    /// </summary>
    /// <param name="pickListId">The pick list ID to retrieve.</param>
    public GetPickListByIdWithItemsSpec(DefaultIdType pickListId)
    {
        Query
            .Where(x => x.Id == pickListId)
            .Include(x => x.Items);
    }
}

