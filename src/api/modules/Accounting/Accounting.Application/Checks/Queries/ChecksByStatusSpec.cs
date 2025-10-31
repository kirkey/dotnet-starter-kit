
namespace Accounting.Application.Checks.Queries;

/// <summary>
/// Specification to find checks by status.
/// </summary>
public class ChecksByStatusSpec : Specification<Check>
{
    public ChecksByStatusSpec(string status)
    {
        Query.Where(c => c.Status == status);
    }
}

