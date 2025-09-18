namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractNotActiveException(DefaultIdType id)
    : Exception($"Wholesale Contract with ID '{id}' is not active.");
