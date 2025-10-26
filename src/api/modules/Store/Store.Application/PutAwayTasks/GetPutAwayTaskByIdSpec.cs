namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks;

/// <summary>
/// Specification to retrieve a put-away task by ID with its items and related entities.
/// </summary>
public sealed class GetPutAwayTaskByIdSpec : Specification<PutAwayTask>, ISingleResultSpecification<PutAwayTask>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPutAwayTaskByIdSpec"/> class.
    /// </summary>
    /// <param name="id">The put-away task ID to retrieve.</param>
    public GetPutAwayTaskByIdSpec(DefaultIdType id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Warehouse)
            .Include(p => p.Items)
                .ThenInclude(item => item.Item)
            .Include(p => p.Items)
                .ThenInclude(item => item.ToBin);
    }
}
