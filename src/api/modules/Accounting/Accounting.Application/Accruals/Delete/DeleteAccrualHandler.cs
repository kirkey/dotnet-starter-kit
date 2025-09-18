namespace Accounting.Application.Accruals.Delete;

using Exceptions;

public class DeleteAccrualHandler(IRepository<Accrual> repository)
    : IRequestHandler<DeleteAccrualRequest, Unit>
{
    public async Task<Unit> Handle(DeleteAccrualRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (accrual == null)
            throw new AccrualNotFoundException(request.Id);

        if (accrual.IsReversed)
            throw new AccrualCannotBeDeletedException(request.Id);

        await repository.DeleteAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

