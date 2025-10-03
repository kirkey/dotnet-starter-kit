namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

public class GetItemByIdSpec : Specification<Item>
{
    public GetItemByIdSpec(DefaultIdType id)
    {
        Query.Where(i => i.Id == id);
    }
}
