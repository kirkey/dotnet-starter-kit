namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;

public class GetSerialNumberByIdSpec : Specification<SerialNumber>
{
    public GetSerialNumberByIdSpec(DefaultIdType id)
    {
        Query
            .Where(s => s.Id == id);
    }
}
