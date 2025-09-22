namespace Accounting.Application.Accruals.Reverse;

/// <summary>
/// Handles reversing an accrual entry by Id.
/// </summary>
public class ReverseAccrualHandler(IRepository<Accrual> repository) : IRequestHandler<ReverseAccrualCommand>
{
    public async Task Handle(ReverseAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccrualByIdNotFoundException(request.Id);

        if (accrual.IsReversed)
            throw new AccrualAlreadyReversedException(request.Id);

        // Apply reversal. Domain will enforce invariants (e.g., cannot reverse twice).
        accrual.Reverse(request.ReversalDate);

        await repository.UpdateAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
