using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Delete.v1;

/// <summary>
/// Handler for deleting payee entities from the accounting system.
/// Implements domain-driven design patterns with proper logging and error handling.
/// </summary>
public sealed class PayeeDeleteHandler(
    ILogger<PayeeDeleteHandler> logger,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeDeleteCommand>
{
    /// <summary>
    /// Handles the deletion of a payee entity.
    /// </summary>
    /// <param name="request">The delete payee command containing the ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="PayeeNotFoundException">Thrown when the payee is not found.</exception>
    public async Task Handle(PayeeDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new PayeeNotFoundException(request.Id);

        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payee with ID {PayeeId} and code {PayeeCode} successfully deleted", entity.Id, entity.PayeeCode);
    }
}
