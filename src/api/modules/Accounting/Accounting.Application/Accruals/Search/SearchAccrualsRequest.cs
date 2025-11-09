using Accounting.Application.Accruals.Responses;
using ValidationException = FluentValidation.ValidationException;

namespace Accounting.Application.Accruals.Search;

/// <summary>
/// Query for searching accruals with pagination and filter options.
/// </summary>
public sealed class SearchAccrualsRequest : PaginationFilter, IRequest<PagedList<AccrualResponse>>
{
    /// <summary>
    /// Partial accrual number to search for. Optional.
    /// </summary>
    public string? NumberLike { get; init; }

    /// <summary>
    /// Start date for accrual search. Optional.
    /// </summary>
    public DateTime? DateFrom { get; init; }

    /// <summary>
    /// End date for accrual search. Optional.
    /// </summary>
    public DateTime? DateTo { get; init; }

    /// <summary>
    /// Filter by reversed status. Optional.
    /// </summary>
    public bool? IsReversed { get; init; }

    /// <summary>
    /// Validates the query parameters for stricter rules.
    /// </summary>
    /// <exception cref="ValidationException">Thrown if validation fails.</exception>
    public void Validate()
    {
        if (PageSize < 1 || PageSize > 100)
            throw new ValidationException("PageSize must be between 1 and 100.");
        if (PageNumber < 1)
            throw new ValidationException("PageNumber must be at least 1.");
        if (DateFrom.HasValue && DateTo.HasValue && DateFrom > DateTo)
            throw new ValidationException("DateFrom cannot be after DateTo.");
    }
}
