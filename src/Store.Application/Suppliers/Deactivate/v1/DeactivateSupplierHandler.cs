using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Deactivate.v1;

/// <summary>
/// Handler to deactivate a supplier aggregate.
/// </summary>
public sealed class DeactivateSupplierHandler(
    ILogger<DeactivateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<DeactivateSupplierCommand, DeactivateSupplierResponse>
{
    public async Task<DeactivateSupplierResponse> Handle(DeactivateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                      ?? throw new SupplierNotFoundException(request.Id);

        supplier.Deactivate();
        await repository.UpdateAsync(supplier, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Supplier with id : {SupplierId} deactivated.", supplier.Id);
        return new DeactivateSupplierResponse(supplier.Id);
    }
}

