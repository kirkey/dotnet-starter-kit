using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Complete.v1;

public sealed class CompleteInventoryTransferHandler(
    ILogger<CompleteInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<CompleteInventoryTransferCommand, CompleteInventoryTransferResponse>
{
    public async Task<CompleteInventoryTransferResponse> Handle(CompleteInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.Id);

        transfer.Complete(request.ActualArrival);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Completed inventory transfer {InventoryTransferId} with arrival {Arrival}", transfer.Id, request.ActualArrival);
        return new CompleteInventoryTransferResponse(transfer.Id);
    }
}

