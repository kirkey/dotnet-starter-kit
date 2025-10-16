using Accounting.Application.Banks.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Delete.v1;

/// <summary>
/// Handler for deleting bank entities from the accounting system.
/// Validates that the bank can be safely deleted before removal.
/// </summary>
public sealed class BankDeleteHandler(
    ILogger<BankDeleteHandler> logger,
    [FromKeyedServices("accounting:banks")] IRepository<Bank> repository)
    : IRequestHandler<BankDeleteCommand, BankDeleteResponse>
{
    /// <summary>
    /// Handles the deletion of a bank entity.
    /// </summary>
    /// <param name="request">The delete bank command containing the bank ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response indicating successful deletion.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="BankNotFoundException">Thrown when the bank is not found.</exception>
    public async Task<BankDeleteResponse> Handle(BankDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bank = await repository.FirstOrDefaultAsync(new BankByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
        _ = bank ?? throw new BankNotFoundException(request.Id);

        // Note: Additional business rule validation could be added here
        // For example, checking if bank has active accounts before deletion

        await repository.DeleteAsync(bank, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Bank deleted with ID {BankId} and code {BankCode}", bank.Id, bank.BankCode);

        return new BankDeleteResponse(request.Id);
    }
}

