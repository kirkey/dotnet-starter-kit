namespace FSH.Starter.WebApi.Store.Application.PickLists;

public sealed class PickListByNumberSpec : Specification<PickList>
{
    public PickListByNumberSpec(string pickListNumber)
    {
        Query.Where(x => x.PickListNumber == pickListNumber);
    }
}
