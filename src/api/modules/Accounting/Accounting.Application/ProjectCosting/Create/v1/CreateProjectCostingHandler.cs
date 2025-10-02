namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Handler for creating project costing entries.
/// </summary>
public sealed class CreateProjectCostingHandler(
    ILogger<CreateProjectCostingHandler> logger,
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> projectRepository,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> coaRepository,
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<CreateProjectCostingCommand, CreateProjectCostingResponse>
{
    /// <summary>
    /// Handles the creation of a new project costing entry.
    /// </summary>
    public async Task<CreateProjectCostingResponse> Handle(CreateProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate project exists
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken).ConfigureAwait(false);
        _ = project ?? throw new Domain.Exceptions.ProjectNotFoundException(request.ProjectId);

        // Validate account exists
        var account = await coaRepository.GetByIdAsync(request.AccountId, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new ChartOfAccountNotFoundException(request.AccountId);

        // Create the project costing entry
        // Signature: projectId, entryDate, amount, description, accountId, category, journalEntryId, costCenter, workOrderNumber, isBillable, vendor, invoiceNumber
        var entry = ProjectCostEntry.Create(
            request.ProjectId,
            request.EntryDate,
            request.Amount,
            request.Description ?? string.Empty,
            request.AccountId,
            request.Category,
            request.JournalEntryId,
            request.CostCenter,
            request.WorkOrderNumber,
            request.IsBillable,
            request.Vendor,
            null); // invoiceNumber not in command, can be added later

        await repository.AddAsync(entry, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Project costing entry created with ID {EntryId} for Project {ProjectId}", entry.Id, entry.ProjectId);

        return new CreateProjectCostingResponse(entry.Id);
    }
}
