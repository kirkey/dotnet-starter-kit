namespace Accounting.Application.Accruals.Create;

using Exceptions;
using Queries;

public sealed class CreateAccrualHandler(
    [FromKeyedServices("accounting:accruals")] IRepository<Accrual> repository)
    : IRequestHandler<CreateAccrualRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateAccrualRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accrualNumber = request.AccrualNumber?.Trim() ?? string.Empty;

        // Duplicate accrual number
        var existing = await repository.FirstOrDefaultAsync(new AccrualByNumberSpec(accrualNumber), cancellationToken);
        if (existing != null)
            throw new AccrualAlreadyExistsException(accrualNumber);

        var accrual = Accrual.Create(accrualNumber, request.AccrualDate, request.Amount, request.Description);
        await repository.AddAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return accrual.Id;
    }
}

