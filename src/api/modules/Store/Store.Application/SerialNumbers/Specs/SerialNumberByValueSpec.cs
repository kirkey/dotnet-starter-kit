namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;

public class SerialNumberByValueSpec : Specification<SerialNumber>
{
    public SerialNumberByValueSpec(string serialNumberValue)
    {
        Query
            .Where(s => s.SerialNumberValue == serialNumberValue);
    }
}
