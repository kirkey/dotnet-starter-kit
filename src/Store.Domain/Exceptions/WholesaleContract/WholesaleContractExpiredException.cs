using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.WholesaleContract;

public sealed class WholesaleContractExpiredException(DefaultIdType id, DateTime expiryDate)
    : ForbiddenException($"Wholesale Contract with ID '{id}' has expired on {expiryDate:yyyy-MM-dd}.") {}
