using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Approve.v1;

public sealed class ApproveInventoryTransferHandler(
    ILogger<ApproveInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<ApproveInventoryTransferCommand, ApproveInventoryTransferResponse>
{
    public async Task<ApproveInventoryTransferResponse> Handle(ApproveInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.Id);

        // domain enforces state
        transfer.Approve(request.ApprovedBy);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Approved inventory transfer {InventoryTransferId} by {ApprovedBy}", transfer.Id, request.ApprovedBy);
        return new ApproveInventoryTransferResponse(transfer.Id);
    }
}

