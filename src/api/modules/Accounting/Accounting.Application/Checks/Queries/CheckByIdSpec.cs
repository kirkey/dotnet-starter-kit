namespace Accounting.Application.Checks.Queries;

/// <summary>
/// Specification to find a check by its ID.
/// </summary>
public class CheckByIdSpec : Specification<Check>, ISingleResultSpecification<Check>
{
    public CheckByIdSpec(DefaultIdType checkId)
    {
        Query.Where(c => c.Id == checkId);
    }
}
