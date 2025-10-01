namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractNotFoundException(DefaultIdType id)
    : NotFoundException($"WholesaleContract with ID {id} was not found.");
