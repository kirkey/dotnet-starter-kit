using Accounting.Application.ChartOfAccounts.Specs;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Payees.Create.v1;

/// <summary>
/// Handler for creating new payee entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class PayeeCreateHandler(
    ILogger<PayeeCreateHandler> logger,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> repositoryCoa,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository,
    IStorageService storageService)
    : IRequestHandler<PayeeCreateCommand, PayeeCreateResponse>
{
    /// <summary>
    /// Handles the creation of a new payee entity.
    /// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    /// <param name="request">The create payee command containing all required information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response containing the ID of the newly created payee.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PayeeCreateResponse> Handle(PayeeCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var coa = await repositoryCoa
            .FirstOrDefaultAsync(new ChartOfAccountByCodeSpec(request.ExpenseAccountCode!), cancellationToken)
            .ConfigureAwait(false);
        _ = coa ?? throw new ChartOfAccountByCodeNotFoundException(request.ExpenseAccountCode!);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Payee>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }
        
        var entity = Payee.Create(
            request.PayeeCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            coa.Name,
            request.Tin,
            request.Description,
            request.Notes,
            imageUrl);

        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payee created with ID {PayeeId} and code {PayeeCode}. ImageUrl: {ImageUrl}", entity.Id, entity.PayeeCode, imageUrl);

        return new PayeeCreateResponse(entity.Id);
    }
}
