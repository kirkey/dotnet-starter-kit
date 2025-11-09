namespace Accounting.Application.Accruals.Reject;

/// <summary>
/// Handler for rejecting an accrual.
/// Validates the accrual exists and is in a state that can be rejected,
/// then marks it as rejected with the rejector's information and reason.
/// </summary>
public sealed class RejectAccrualHandler(
    ILogger<RejectAccrualHandler> logger,
    [FromKeyedServices("accounting:accruals")] IRepository<Accrual> repository)
    : IRequestHandler<RejectAccrualCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the rejection of an accrual.
    /// </summary>
    /// <param name="request">The reject accrual command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the rejected accrual.</returns>
    /// <exception cref="AccrualByIdNotFoundException">Thrown when the accrual is not found.</exception>
    /// <exception cref="AccrualAlreadyReversedException">Thrown when the accrual is already reversed.</exception>
    public async Task<DefaultIdType> Handle(RejectAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.AccrualId, cancellationToken)
            ?? throw new AccrualByIdNotFoundException(request.AccrualId);

        // Reject the accrual using domain method
        accrual.Reject(request.RejectedBy, request.Reason);

        await repository.UpdateAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Accrual {AccrualNumber} (ID: {AccrualId}) rejected by {RejectedBy}. Reason: {Reason}",
            accrual.AccrualNumber,
            accrual.Id,
            request.RejectedBy,
            request.Reason ?? "Not specified");

        return accrual.Id;
    }
}
