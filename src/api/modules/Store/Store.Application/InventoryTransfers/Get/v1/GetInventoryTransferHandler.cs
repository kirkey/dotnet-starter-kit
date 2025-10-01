using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;
using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;

public sealed class GetInventoryTransferHandler(
    ILogger<GetInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<GetInventoryTransferQuery, GetInventoryTransferResponse>
{
    public async Task<GetInventoryTransferResponse> Handle(GetInventoryTransferQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var spec = new GetInventoryTransferSpecification(request.Id);
        var inventoryTransfer = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = inventoryTransfer ?? throw new InventoryTransferNotFoundException(request.Id);
        
        logger.LogInformation("Retrieved inventory transfer {InventoryTransferId}", inventoryTransfer.Id);
        
        return inventoryTransfer.Adapt<GetInventoryTransferResponse>();
    }
}
