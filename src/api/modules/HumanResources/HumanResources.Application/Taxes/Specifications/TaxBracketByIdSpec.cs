namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting tax bracket by ID.
/// </summary>
public sealed class TaxBracketByIdSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    public TaxBracketByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
