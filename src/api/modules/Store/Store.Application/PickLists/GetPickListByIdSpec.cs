namespace FSH.Starter.WebApi.Store.Application.PickLists;

public sealed class GetPickListByIdSpec : Specification<PickList>
{
    public GetPickListByIdSpec(DefaultIdType pickListId)
    {
        Query
            .Where(x => x.Id == pickListId)
            .Include(x => x.Items);
    }
}
