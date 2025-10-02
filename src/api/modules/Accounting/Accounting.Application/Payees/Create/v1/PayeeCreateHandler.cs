using Accounting.Application.ChartOfAccounts.Specs;

namespace Accounting.Application.Payees.Create.v1;

/// <summary>
/// Handler for creating new payee entities in the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class PayeeCreateHandler(
    ILogger<PayeeCreateHandler> logger,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> repositoryCoa,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeCreateCommand, PayeeCreateResponse>
{
    /// <summary>
    /// Handles the creation of a new payee entity.
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
        
        var entity = Payee.Create(
            request.PayeeCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            coa.Name,
            request.Tin,
            request.Description,
            request.Notes);

        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payee created with ID {PayeeId} and code {PayeeCode}", entity.Id, entity.PayeeCode);

        return new PayeeCreateResponse(entity.Id);
    }
}
