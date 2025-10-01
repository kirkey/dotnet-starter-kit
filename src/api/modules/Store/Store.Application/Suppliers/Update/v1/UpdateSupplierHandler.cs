using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Handles updates to Supplier aggregates.
/// </summary>
public sealed class UpdateSupplierHandler(
    ILogger<UpdateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<UpdateSupplierCommand, UpdateSupplierResponse>
{
    public async Task<UpdateSupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = supplier ?? throw new SupplierNotFoundException(request.Id);

        var updated = supplier.Update(
            request.Name,
            request.Description,
            request.ContactPerson,
            request.Email,
            request.Phone,
            request.Address,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.Website,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.Rating,
            request.Notes);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Updated supplier {SupplierId}", supplier.Id);
        return new UpdateSupplierResponse(supplier.Id);
    }
}
