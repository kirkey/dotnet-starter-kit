using Accounting.Application.Accruals.Commands;
using Accounting.Application.Accruals.Exceptions;

namespace Accounting.Application.Accruals.Handlers;

public class DeleteAccrualHandler(
    IRepository<Accrual> repository)
    : IRequestHandler<DeleteAccrualCommand>
{
    public async Task Handle(DeleteAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (accrual == null)
            throw new AccrualNotFoundException(request.Id);

        if (accrual.IsReversed)
            throw new AccrualCannotBeDeletedException(request.Id);

        await repository.DeleteAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
