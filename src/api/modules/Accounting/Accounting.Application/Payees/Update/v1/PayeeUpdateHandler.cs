using Accounting.Application.ChartOfAccounts.Specs;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Payees.Update.v1;

/// <summary>
/// Handler for updating existing payee entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// If an image payload is provided by the client, it uploads the image using the configured storage service
/// and sets the Payee.ImageUrl to the saved file's public URI.
/// </summary>
public sealed class PayeeUpdateHandler(
    ILogger<PayeeUpdateHandler> logger,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> repositoryCoa,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository,
    IStorageService storageService)
    : IRequestHandler<PayeeUpdateCommand, PayeeUpdateResponse>
{
    /// <summary>
    /// Handles the update of an existing payee entity.
    /// Processes the update command, optionally handling image upload and setting the resulting ImageUrl.
    /// Validates inputs and ensures that when an image payload is provided it is saved and the resulting
    /// absolute URI (as returned by the storage provider) is persisted on the Payee.
    /// </summary>
    /// <param name="request">The update payee command containing all information to update.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response containing the ID of the updated payee.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="PayeeNotFoundException">Thrown when the payee is not found.</exception>
    public async Task<PayeeUpdateResponse> Handle(PayeeUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = payee ?? throw new PayeeNotFoundException(request.Id);
        
        var coa = await repositoryCoa
            .FirstOrDefaultAsync(new ChartOfAccountByCodeSpec(request.ExpenseAccountCode!), cancellationToken)
            .ConfigureAwait(false);
        _ = coa ?? throw new ChartOfAccountByCodeNotFoundException(request.ExpenseAccountCode!);

        // Determine the image URL to save on the Payee.
        // Priority: uploaded image URI > explicitly provided ImageUrl > keep existing value.
        string? imageUrl = payee.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Payee>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            // Guard: storage provider should return a URI for a successful upload
            _ = uri ?? throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");

            // Persist the full absolute URI returned by the storage provider so it can be used by clients.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }
        else if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            imageUrl = request.ImageUrl;
        }

        var updatedPayee = payee.Update(
            request.PayeeCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            coa.Name,
            request.Tin,
            request.Description,
            request.Notes,
            imageUrl);

        await repository.UpdateAsync(updatedPayee, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payee with ID {PayeeId} and code {PayeeCode} updated successfully. ImageUrl set: {ImageUrl}", updatedPayee.Id, updatedPayee.PayeeCode, imageUrl);

        return new PayeeUpdateResponse(updatedPayee.Id);
    }
}
