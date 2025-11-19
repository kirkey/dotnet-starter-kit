namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Specifications;

/// <summary>
/// Specification for getting a shift by ID.
/// </summary>
public class ShiftByIdSpec : Specification<Shift>, ISingleResultSpecification<Shift>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShiftByIdSpec"/> class.
    /// </summary>
    public ShiftByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching shifts with filters.
/// </summary>
public class SearchShiftsSpec : Specification<Shift>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchShiftsSpec"/> class.
    /// </summary>
    public SearchShiftsSpec(Search.v1.SearchShiftsRequest request)
    {
        Query
            .OrderBy(x => x.ShiftName);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.ShiftName.Contains(request.SearchString) ||
                           x.Description!.Contains(request.SearchString));

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);
    }
}

