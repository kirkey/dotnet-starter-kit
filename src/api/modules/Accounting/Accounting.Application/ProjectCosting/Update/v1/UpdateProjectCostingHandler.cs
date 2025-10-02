namespace Accounting.Application.ProjectCosting.Update.v1;

/// <summary>
/// Handler for updating project costing entries.
/// </summary>
public sealed class UpdateProjectCostingHandler(
    ILogger<UpdateProjectCostingHandler> logger,
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<UpdateProjectCostingCommand, UpdateProjectCostingResponse>
{
    /// <summary>
    /// Handles the update of an existing project costing entry.
    /// </summary>
    public async Task<UpdateProjectCostingResponse> Handle(UpdateProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entry ?? throw new Domain.Exceptions.ProjectCostNotFoundException(request.Id);

        var updatedEntry = entry.Update(
            request.EntryDate,
            request.Amount,
            request.Description,
            request.Category,
            request.CostCenter,
            request.WorkOrderNumber,
            request.IsBillable,
            request.Vendor,
            request.InvoiceNumber);

        await repository.UpdateAsync(updatedEntry, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Project costing entry with ID {EntryId} updated successfully", updatedEntry.Id);

        return new UpdateProjectCostingResponse(updatedEntry.Id);
    }
}
