using Accounting.Application.Currencies.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Currencies.Queries;

public sealed class CurrencyByIdSpec :
    Specification<Currency, CurrencyDto>,
    ISingleResultSpecification<Currency, CurrencyDto>
{
    public CurrencyByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
