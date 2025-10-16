using Accounting.Application.Projects.Get.v1;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Specifications;

/// <summary>
/// Specification to retrieve a project by ID projected to <see cref="GetProjectResponse"/>.
/// This specification performs database-level projection for optimal performance.
/// </summary>
public sealed class GetProjectSpec : Specification<Project, GetProjectResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectSpec"/> class.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project to retrieve.</param>
    public GetProjectSpec(DefaultIdType projectId)
    {
        Query
            .Where(p => p.Id == projectId)
            .Include(p => p.CostingEntries);

        Query.Select(p => new GetProjectResponse(
            p.Id,
            p.Name,
            p.StartDate,
            p.EndDate,
            p.BudgetedAmount,
            p.Status,
            p.ClientName,
            p.ProjectManager,
            p.Department,
            p.Description,
            p.Notes,
            p.ImageUrl,
            p.ActualCost,
            p.ActualRevenue,
            p.BudgetVariance,
            p.BudgetUtilizationPercentage,
            p.ProfitLoss,
            p.CostingEntries.Select(ce => new ProjectCostSummary(
                ce.Id,
                ce.EntryDate,
                ce.Description ?? string.Empty,
                ce.Amount,
                ce.Category,
                ce.CostCenter,
                ce.IsBillable,
                ce.IsApproved
            )),
            p.CreatedOn.DateTime,
            p.LastModifiedOn.DateTime
        ));
    }
}

