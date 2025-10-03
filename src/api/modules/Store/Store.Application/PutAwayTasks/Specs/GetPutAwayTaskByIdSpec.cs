namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Specs;

/// <summary>
/// Specification to get a put-away task by its ID with related data.
/// </summary>
public sealed class GetPutAwayTaskByIdSpec : Specification<PutAwayTask>
{
    public GetPutAwayTaskByIdSpec(DefaultIdType putAwayTaskId)
    {
        Query
            .Where(p => p.Id == putAwayTaskId)
            .Include(p => p.Warehouse)
            .Include(p => p.GoodsReceipt)
            .Include(p => p.Items);
    }
}
