using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Cancel.v1;

public sealed class CancelInventoryTransferHandler(
    ILogger<CancelInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<CancelInventoryTransferCommand, CancelInventoryTransferResponse>
{
    public async Task<CancelInventoryTransferResponse> Handle(CancelInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.Id);

        transfer.Cancel(request.Reason);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cancelled inventory transfer {InventoryTransferId} Reason: {Reason}", transfer.Id, request.Reason);
        return new CancelInventoryTransferResponse(transfer.Id);
    }
}

