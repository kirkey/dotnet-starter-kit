namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Specifications;

using Domain.Entities;
using Search.v1;

/// <summary>
/// Specification for getting a deduction by ID.
/// </summary>
public sealed class DeductionByIdSpec : Specification<Deduction>
{
    public DeductionByIdSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}

/// <summary>
/// Specification for searching deductions.
/// </summary>
public sealed class SearchDeductionsSpec : Specification<Deduction>
{
    public SearchDeductionsSpec(SearchDeductionsRequest request)
    {
        Query.OrderBy(d => d.DeductionName);

        if (!string.IsNullOrWhiteSpace(request.DeductionType))
            Query.Where(d => d.DeductionType == request.DeductionType);

        if (!string.IsNullOrWhiteSpace(request.RecoveryMethod))
            Query.Where(d => d.RecoveryMethod == request.RecoveryMethod);

        if (request.IsActive.HasValue)
            Query.Where(d => d.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            Query.Search(d => d.DeductionName!, $"%{request.SearchTerm}%");

        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

/// <summary>
/// Specification for getting active deductions.
/// </summary>
public sealed class ActiveDeductionsSpec : Specification<Deduction>
{
    public ActiveDeductionsSpec()
    {
        Query.Where(d => d.IsActive)
            .OrderBy(d => d.DeductionName);
    }
}

/// <summary>
/// Specification for getting deductions by type.
/// </summary>
public sealed class DeductionsByTypeSpec : Specification<Deduction>
{
    public DeductionsByTypeSpec(string deductionType)
    {
        Query.Where(d => d.DeductionType == deductionType)
            .OrderBy(d => d.DeductionName);
    }
}

