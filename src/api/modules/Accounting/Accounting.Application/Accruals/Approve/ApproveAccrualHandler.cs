using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.Accruals.Approve;

/// <summary>
/// Handler for approving an accrual.
/// Validates the accrual exists and is in a state that can be approved,
/// then marks it as approved with the approver's information from the current user session.
/// </summary>
public sealed class ApproveAccrualHandler(
    ILogger<ApproveAccrualHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:accruals")] IRepository<Accrual> repository)
    : IRequestHandler<ApproveAccrualCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the approval of an accrual.
    /// </summary>
    /// <param name="request">The approve accrual command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the approved accrual.</returns>
    /// <exception cref="AccrualByIdNotFoundException">Thrown when the accrual is not found.</exception>
    /// <exception cref="AccrualAlreadyApprovedException">Thrown when the accrual is already approved.</exception>
    /// <exception cref="AccrualAlreadyReversedException">Thrown when the accrual is already reversed.</exception>
    public async Task<DefaultIdType> Handle(ApproveAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.AccrualId, cancellationToken)
            ?? throw new AccrualByIdNotFoundException(request.AccrualId);

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        // Approve the accrual using domain method
        accrual.Approve(approverId, approverName);

        await repository.UpdateAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Accrual {AccrualNumber} (ID: {AccrualId}) approved by {ApprovedBy}",
            accrual.AccrualNumber,
            accrual.Id,
            approverName);

        return accrual.Id;
    }
}
