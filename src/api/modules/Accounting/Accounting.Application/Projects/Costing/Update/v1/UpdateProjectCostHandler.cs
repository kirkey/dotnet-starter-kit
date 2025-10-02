namespace Accounting.Application.Projects.Costing.Update.v1;

/// <summary>
/// Handler for updating existing project cost entries with proper domain validation.
/// </summary>
public sealed class UpdateProjectCostHandler(
    ILogger<UpdateProjectCostHandler> logger,
    [FromKeyedServices("accounting:projectcosts")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<UpdateProjectCostCommand, UpdateProjectCostResponse>
{
    public async Task<UpdateProjectCostResponse> Handle(UpdateProjectCostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var projectCost = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectCostNotFoundException(request.Id);

        // Update the project cost using a domain method
        projectCost.Update(
            request.EntryDate,
            request.Amount,
            request.Description,
            request.Category,
            request.CostCenter,
            request.WorkOrderNumber,
            request.IsBillable,
            request.Vendor,
            request.InvoiceNumber);

        await repository.UpdateAsync(projectCost, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project cost updated {ProjectCostId}", projectCost.Id);

        return new UpdateProjectCostResponse(projectCost.Id);
    }
}
