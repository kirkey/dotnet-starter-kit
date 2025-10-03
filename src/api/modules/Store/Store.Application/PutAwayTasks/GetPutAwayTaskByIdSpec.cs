namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks;

public sealed class GetPutAwayTaskByIdSpec : Specification<PutAwayTask>, ISingleResultSpecification<PutAwayTask>
{
    public GetPutAwayTaskByIdSpec(DefaultIdType id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Items);
    }
}
