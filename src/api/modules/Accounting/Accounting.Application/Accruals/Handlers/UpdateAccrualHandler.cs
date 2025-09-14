using Accounting.Application.Accruals.Commands;
using Accounting.Application.Accruals.Exceptions;
using Accounting.Application.Accruals.Queries;

namespace Accounting.Application.Accruals.Handlers;

public class UpdateAccrualHandler(
    IRepository<Accrual> repository)
    : IRequestHandler<UpdateAccrualCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateAccrualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (accrual == null)
            throw new AccrualNotFoundException(request.Id);

        // If accrualNumber is being changed, ensure uniqueness
        if (!string.IsNullOrWhiteSpace(request.AccrualNumber))
        {
            var existing = await repository.FirstOrDefaultAsync(
                new AccrualByNumberSpec(request.AccrualNumber, request.Id), cancellationToken);
            if (existing != null && existing.Id != request.Id)
                throw new AccrualAlreadyExistsException(request.AccrualNumber!);
        }

        accrual.Update(request.AccrualNumber, request.AccrualDate, request.Amount, request.Description);

        await repository.UpdateAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return accrual.Id;
    }
}
