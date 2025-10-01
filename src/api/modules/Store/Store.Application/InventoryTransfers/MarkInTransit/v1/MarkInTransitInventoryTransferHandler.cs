using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.MarkInTransit.v1;

public sealed class MarkInTransitInventoryTransferHandler(
    ILogger<MarkInTransitInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<MarkInTransitInventoryTransferCommand, MarkInTransitInventoryTransferResponse>
{
    public async Task<MarkInTransitInventoryTransferResponse> Handle(MarkInTransitInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.Id);

        transfer.MarkInTransit();
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Marked inventory transfer {InventoryTransferId} as InTransit", transfer.Id);
        return new MarkInTransitInventoryTransferResponse(transfer.Id);
    }
}

