using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.PriceList;

public sealed class PriceListNotFoundException(DefaultIdType id)
    : NotFoundException($"Price List with ID '{id}' was not found.") {}
