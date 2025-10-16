using Accounting.Domain.Entities;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Banks.Create.v1;

/// <summary>
/// Handler for creating new bank entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class BankCreateHandler(
    ILogger<BankCreateHandler> logger,
    [FromKeyedServices("accounting:banks")] IRepository<Bank> repository,
    IStorageService storageService)
    : IRequestHandler<BankCreateCommand, BankCreateResponse>
{
    /// <summary>
    /// Handles the creation of a new bank entity.
    /// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    /// <param name="request">The create bank command containing all required information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response containing the ID of the newly created bank.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<BankCreateResponse> Handle(BankCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Bank>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

        var entity = Bank.Create(
            request.BankCode,
            request.Name,
            request.RoutingNumber,
            request.SwiftCode,
            request.Address,
            request.ContactPerson,
            request.PhoneNumber,
            request.Email,
            request.Website,
            request.Description,
            request.Notes,
            imageUrl);

        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Bank created with ID {BankId} and code {BankCode}. ImageUrl: {ImageUrl}", entity.Id, entity.BankCode, imageUrl);

        return new BankCreateResponse(entity.Id);
    }
}
