using Accounting.Application.Currencies.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Currencies.Queries;

public sealed class CurrencyByNameSpec :
    Specification<Currency, CurrencyDto>,
    ISingleResultSpecification<Currency, CurrencyDto>
{
    public CurrencyByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
