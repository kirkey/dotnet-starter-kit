using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

/// <summary>
/// Handles the creation of Supplier aggregates.
/// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
/// </summary>
public sealed class CreateSupplierHandler(
    ILogger<CreateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository,
    IStorageService storageService)
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    public async Task<CreateSupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

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

        var supplier = Supplier.Create(
            request.Name,
            request.Description,
            request.Code,
            request.ContactPerson,
            request.Email,
            request.Phone,
            request.Address,
            request.PostalCode,
            request.Website,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.IsActive,
            request.Rating,
            request.Notes
        );

        // Set the image URL after creation
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            supplier.ImageUrl = imageUrl;
        }

        await repository.AddAsync(supplier, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Supplier created {SupplierId}. ImageUrl: {ImageUrl}", supplier.Id, imageUrl);
        return new CreateSupplierResponse(supplier.Id);
    }
}
