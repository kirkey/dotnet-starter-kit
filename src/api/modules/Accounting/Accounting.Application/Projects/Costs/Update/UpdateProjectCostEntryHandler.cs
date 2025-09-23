using Accounting.Application.Projects.Queries;

namespace Accounting.Application.Projects.Costs.Update;

/// <summary>
/// Handles updating an existing job cost entry within a project aggregate.
/// </summary>
public sealed class UpdateProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository
) : IRequestHandler<UpdateProjectCostEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateProjectCostEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        var entry = project.CostingEntries.FirstOrDefault(e => e.Id == request.EntryId)
                   ?? throw new JobCostingEntryNotFoundException(request.EntryId);

        // Budget enforcement when amount changes for cost entries
        if (request.Amount.HasValue && entry.Amount > 0)
        {
            var oldAmount = entry.Amount;
            var newAmount = request.Amount.Value;
            if (newAmount <= 0) throw new InvalidProjectCostEntryException();

            var projectedActualCost = project.ActualCost - oldAmount + newAmount;
            if (project.BudgetedAmount > 0 && projectedActualCost > project.BudgetedAmount)
                throw new ProjectBudgetExceededException(newAmount, project.BudgetedAmount, project.ActualCost);
        }

        project.UpdateCostEntry(request.EntryId, request.Date, request.Description, request.Amount, request.Category);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return request.EntryId;
    }
}
