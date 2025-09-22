namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractNotActiveException(DefaultIdType id)
    : CustomException($"Wholesale Contract with ID '{id}' is not active.");
