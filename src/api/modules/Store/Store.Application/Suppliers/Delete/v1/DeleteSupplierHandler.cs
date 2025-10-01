using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Delete.v1;

/// <summary>
/// Handles deletion of Supplier aggregates after validation.
/// </summary>
public sealed class DeleteSupplierHandler(
    ILogger<DeleteSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<DeleteSupplierCommand>
{
    public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = supplier ?? throw new SupplierNotFoundException(request.Id);
        await repository.DeleteAsync(supplier, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Deleted supplier {SupplierId}", supplier.Id);
    }
}
