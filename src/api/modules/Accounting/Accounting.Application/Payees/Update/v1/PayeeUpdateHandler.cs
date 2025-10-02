using Accounting.Application.ChartOfAccounts.Specs;

namespace Accounting.Application.Payees.Update.v1;

/// <summary>
/// Handler for updating existing payee entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class PayeeUpdateHandler(
    ILogger<PayeeUpdateHandler> logger,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> repositoryCoa,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeUpdateCommand, PayeeUpdateResponse>
{
    /// <summary>
    /// Handles the update of an existing payee entity.
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

        var updatedPayee = payee.Update(
            request.PayeeCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            coa.Name,
            request.Tin,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(updatedPayee, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payee with ID {PayeeId} and code {PayeeCode} updated successfully", updatedPayee.Id, updatedPayee.PayeeCode);

        return new PayeeUpdateResponse(updatedPayee.Id);
    }
}
