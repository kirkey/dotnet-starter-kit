using Accounting.Application.Banks.Specs;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Handler for updating existing bank entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class UpdateBankHandler(
    ILogger<UpdateBankHandler> logger,
    [FromKeyedServices("accounting:banks")] IRepository<Bank> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateBankCommand, BankUpdateResponse>
{
    /// <summary>
    /// Handles the update of an existing bank entity.
    /// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    /// <param name="request">The update bank command containing all updated information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response indicating successful update.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="BankNotFoundException">Thrown when the bank is not found.</exception>
    public async Task<BankUpdateResponse> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bank = await repository.FirstOrDefaultAsync(new BankByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
        _ = bank ?? throw new BankNotFoundException(request.Id);

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

        bank.Update(
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

        await repository.UpdateAsync(bank, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Bank updated with ID {BankId} and code {BankCode}. ImageUrl: {ImageUrl}", bank.Id, bank.BankCode, imageUrl);

        return new BankUpdateResponse(bank.Id);
    }
}

