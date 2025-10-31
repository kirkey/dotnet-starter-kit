namespace Accounting.Application.RegulatoryReports.Queries;

/// <summary>
/// Specification to retrieve a regulatory report entity by ID for update/delete operations.
/// Returns the full entity with all navigation properties needed for domain operations.
/// </summary>
public class RegulatoryReportByIdEntitySpec : SingleResultSpecification<RegulatoryReport>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegulatoryReportByIdEntitySpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the regulatory report to retrieve.</param>
    public RegulatoryReportByIdEntitySpec(DefaultIdType id)
    {
        Query.Where(r => r.Id == id);
    }
}

