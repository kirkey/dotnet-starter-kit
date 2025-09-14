namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractNotFoundException : NotFoundException
{
    public WholesaleContractNotFoundException(DefaultIdType id) : base($"WholesaleContract with ID {id} was not found.") { }
}
