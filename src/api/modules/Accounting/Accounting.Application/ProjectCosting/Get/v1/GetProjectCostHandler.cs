namespace Accounting.Application.ProjectCosting.Get.v1;

/// <summary>
/// Handler for retrieving project cost entry details.
/// </summary>
public sealed class GetProjectCostHandler(
    ILogger<GetProjectCostHandler> logger,
    [FromKeyedServices("accounting:projectcosts")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<GetProjectCostQuery, GetProjectCostResponse>
{
    public async Task<GetProjectCostResponse> Handle(GetProjectCostQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cost = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectCostNotFoundException(request.Id);

        logger.LogInformation("project cost retrieved {ProjectCostId}", cost.Id);

        return new GetProjectCostResponse(
            ProjectCostId: cost.Id,
            ProjectId: cost.ProjectId,
            EntryDate: cost.EntryDate,
            Amount: cost.Amount,
            Description: cost.Description ?? string.Empty,
            Category: cost.Category,
            AccountId: cost.AccountId,
            JournalEntryId: cost.JournalEntryId,
            CostCenter: cost.CostCenter,
            WorkOrderNumber: cost.WorkOrderNumber,
            IsBillable: cost.IsBillable,
            IsApproved: cost.IsApproved,
            Vendor: cost.Vendor,
            InvoiceNumber: cost.InvoiceNumber,
            CreatedOn: cost.CreatedOn,
            LastModifiedOn: cost.LastModifiedOn);
    }
}
