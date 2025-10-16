using Accounting.Application.Banks.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Handler for updating existing bank entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class BankUpdateHandler(
    ILogger<BankUpdateHandler> logger,
    [FromKeyedServices("accounting:banks")] IRepository<Bank> repository)
    : IRequestHandler<BankUpdateCommand, BankUpdateResponse>
{
    /// <summary>
    /// Handles the update of an existing bank entity.
    /// </summary>
    /// <param name="request">The update bank command containing all updated information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response indicating successful update.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="BankNotFoundException">Thrown when the bank is not found.</exception>
    public async Task<BankUpdateResponse> Handle(BankUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bank = await repository.FirstOrDefaultAsync(new BankByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
        _ = bank ?? throw new BankNotFoundException(request.Id);

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
            request.ImageUrl);

        await repository.UpdateAsync(bank, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Bank updated with ID {BankId} and code {BankCode}", bank.Id, bank.BankCode);

        return new BankUpdateResponse(bank.Id);
    }
}

