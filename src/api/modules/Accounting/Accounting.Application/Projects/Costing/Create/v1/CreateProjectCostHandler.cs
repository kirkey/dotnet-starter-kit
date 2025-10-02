namespace Accounting.Application.Projects.Costing.Create.v1;

/// <summary>
/// Handler for creating new project cost entries with proper domain validation and event publishing.
/// </summary>
public sealed class CreateProjectCostHandler(
    ILogger<CreateProjectCostHandler> logger,
    [FromKeyedServices("accounting:projectcosts")] IRepository<ProjectCostEntry> projectCostRepository,
    [FromKeyedServices("accounting:projects")] IRepository<Project> projectRepository)
    : IRequestHandler<CreateProjectCostCommand, CreateProjectCostResponse>
{
    public async Task<CreateProjectCostResponse> Handle(CreateProjectCostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify the project exists
        _ = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.ProjectId);

        // Create the project cost using a domain factory method
        var projectCost = ProjectCostEntry.Create(
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

        // Save to repository
        await projectCostRepository.AddAsync(projectCost, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project cost created {ProjectCostId} for project {ProjectId}", projectCost.Id, projectCost.ProjectId);

        return new CreateProjectCostResponse(projectCost.Id);
    }
}
