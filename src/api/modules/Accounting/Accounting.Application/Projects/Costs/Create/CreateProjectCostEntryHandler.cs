using Accounting.Application.Projects.Queries;

namespace Accounting.Application.Projects.Costs.Create;

/// <summary>
/// Handles creation of a new job cost entry for a project aggregate.
/// </summary>
public sealed class CreateProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository
) : IRequestHandler<CreateProjectCostEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateProjectCostEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        // Strict budget validation: prevent exceeding budget unless budget is 0 (no budget)
        if (project.BudgetedAmount > 0 && project.ActualCost + request.Amount > project.BudgetedAmount)
            throw new ProjectBudgetExceededException(request.Amount, project.BudgetedAmount, project.ActualCost);

        project.AddCostEntry(
            request.Date,
            request.Description,
            request.Amount,
            request.AccountId,
            request.JournalEntryId,
            request.Category);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        // Retrieve the just-added entry from the in-memory collection
        var newEntry = project.CostingEntries
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .FirstOrDefault(e => e.Amount == request.Amount
                              && e.Description == request.Description.Trim()
                              && e.AccountId == request.AccountId
                              && e.JournalEntryId == request.JournalEntryId);

        return newEntry?.Id ?? DefaultIdType.Empty;
    }
}
