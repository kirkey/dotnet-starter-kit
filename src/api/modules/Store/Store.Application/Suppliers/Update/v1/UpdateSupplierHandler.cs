using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Handles updates to Supplier aggregates.
/// Handles image upload if provided.
/// </summary>
public sealed class UpdateSupplierHandler(
    ILogger<UpdateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateSupplierCommand, UpdateSupplierResponse>
{
    public async Task<UpdateSupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = supplier ?? throw new SupplierNotFoundException(request.Id);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Supplier>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

        var updated = supplier.Update(
            request.Code,
            request.Name,
            request.Description,
            request.ContactPerson,
            request.Email,
            request.Phone,
            request.Address,
            request.PostalCode,
            request.Website,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.Rating,
            request.Notes);

        // Update image URL if changed
        if (!string.IsNullOrWhiteSpace(imageUrl) && updated.ImageUrl != imageUrl)
        {
            updated.ImageUrl = imageUrl;
        }

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Updated supplier {SupplierId}. ImageUrl: {ImageUrl}", supplier.Id, imageUrl);
        return new UpdateSupplierResponse(supplier.Id);
    }
}
