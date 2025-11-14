namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting tax brackets by type and year.
/// Used for payroll tax calculation.
/// </summary>
public sealed class TaxBracketsByTypeAndYearSpec : Specification<TaxBracket>
{
    public TaxBracketsByTypeAndYearSpec(string taxType, int year)
    {
        Query.Where(x => x.TaxType == taxType && x.Year == year)
            .OrderBy(x => x.MinIncome);
    }
}

