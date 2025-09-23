namespace Accounting.Application.Projects.Get.v1;

/// <summary>
/// Response for the get project query containing comprehensive project details and cost information.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project</param>
/// <param name="Name">The name of the project</param>
/// <param name="StartDate">The project start date</param>
/// <param name="EndDate">The project end date (if completed/cancelled)</param>
/// <param name="BudgetedAmount">The approved budget amount</param>
/// <param name="Status">The current project status</param>
/// <param name="ClientName">The client or customer name</param>
/// <param name="ProjectManager">The assigned project manager</param>
/// <param name="Department">The owning department</param>
/// <param name="Description">The project description</param>
/// <param name="Notes">The project notes</param>
/// <param name="ActualCost">The current actual cost</param>
/// <param name="ActualRevenue">The current actual revenue</param>
/// <param name="BudgetVariance">The current budget variance</param>
/// <param name="BudgetUtilizationPercentage">The percentage of budget utilized</param>
/// <param name="ProfitLoss">The current profit/loss calculation</param>
/// <param name="CostEntries">List of associated cost entries</param>
/// <param name="CreatedOn">When the project was created</param>
/// <param name="LastModifiedOn">When the project was last modified</param>
public sealed record GetProjectResponse(
    DefaultIdType ProjectId,
    string Name,
    DateTime StartDate,
    DateTime? EndDate,
    decimal BudgetedAmount,
    string Status,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    string? Description,
    string? Notes,
    decimal ActualCost,
    decimal ActualRevenue,
    decimal BudgetVariance,
    decimal BudgetUtilizationPercentage,
    decimal ProfitLoss,
    IEnumerable<ProjectCostSummary> CostEntries,
    DateTime CreatedOn,
    DateTime? LastModifiedOn);

/// <summary>
/// Summary information for project cost entries.
/// </summary>
/// <param name="Id">The cost entry identifier</param>
/// <param name="EntryDate">The date of the cost entry</param>
/// <param name="Description">The description of the cost</param>
/// <param name="Amount">The cost amount</param>
/// <param name="Category">The cost category</param>
/// <param name="CostCenter">The cost center allocation</param>
/// <param name="IsBillable">Whether the cost is billable</param>
/// <param name="IsApproved">Whether the cost is approved</param>
public sealed record ProjectCostSummary(
    DefaultIdType Id,
    DateTime EntryDate,
    string Description,
    decimal Amount,
    string? Category,
    string? CostCenter,
    bool IsBillable,
    bool IsApproved);
