using Accounting.Application.Currencies.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Currencies.Queries;

public sealed class CurrencyByCodeSpec :
    Specification<Currency, CurrencyDto>,
    ISingleResultSpecification<Currency, CurrencyDto>
{
    public CurrencyByCodeSpec(string code) =>
        Query.Where(w => w.CurrencyCode == code);
}
