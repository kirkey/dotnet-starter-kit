namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractNotActiveException : Exception
{
    public WholesaleContractNotActiveException(DefaultIdType id)
        : base($"Wholesale Contract with ID '{id}' is not active.") {}
}
