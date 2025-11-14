namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting applicable tax bracket for an income amount.
/// </summary>
public sealed class TaxBracketByIncomeSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    public TaxBracketByIncomeSpec(string taxType, int year, decimal income, string? filingStatus = null)
    {
        Query.Where(x => x.TaxType == taxType &&
                        x.Year == year &&
                        x.MinIncome <= income &&
                        x.MaxIncome > income);

        if (!string.IsNullOrWhiteSpace(filingStatus))
            Query.Where(x => x.FilingStatus == filingStatus || x.FilingStatus == null);

        Query.OrderBy(x => x.MinIncome);
    }
}

