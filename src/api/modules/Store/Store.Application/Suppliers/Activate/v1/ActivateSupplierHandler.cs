using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;

/// <summary>
/// Handler to activate a supplier aggregate.
/// </summary>
public sealed class ActivateSupplierHandler(
    ILogger<ActivateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<ActivateSupplierCommand, ActivateSupplierResponse>
{
    public async Task<ActivateSupplierResponse> Handle(ActivateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                      ?? throw new SupplierNotFoundException(request.Id);

        supplier.Activate();
        await repository.UpdateAsync(supplier, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Supplier with id : {SupplierId} activated.", supplier.Id);
        return new ActivateSupplierResponse(supplier.Id);
    }
}

