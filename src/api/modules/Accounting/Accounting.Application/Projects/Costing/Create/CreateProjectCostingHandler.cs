using Accounting.Application.Projects.Exceptions;

namespace Accounting.Application.Projects.Costing.Create;

/// <summary>
/// Handles creation of a new project costing entry.
/// </summary>
/// <remarks>
/// Validates that:
/// - The project exists and is active.
/// - The account exists and is valid.
/// - The cost amount is positive.
/// On success, creates the project cost entry and persists changes.
/// </remarks>
public sealed class CreateProjectCostingHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> projectRepository,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> accountRepository,
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<CreateProjectCostingCommand, DefaultIdType>
{
    /// <summary>
    /// Creates the project costing entry.
    /// </summary>
    public async Task<DefaultIdType> Handle(CreateProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate project exists
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new Accounting.Domain.Exceptions.ProjectNotFoundException(request.ProjectId);

        // Validate account exists
        var account = await accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
            throw new ChartOfAccountNotFoundException(request.AccountId);

        // Create the project cost entry using the domain factory method
        var entry = ProjectCostEntry.Create(
            request.ProjectId,
            request.EntryDate,
            request.Amount,
            request.Description,
            request.AccountId,
            request.Category,
            request.JournalEntryId,
            request.CostCenter,
            request.WorkOrderNumber,
            request.IsBillable,
            request.Vendor,
            request.InvoiceNumber);

        await repository.AddAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
