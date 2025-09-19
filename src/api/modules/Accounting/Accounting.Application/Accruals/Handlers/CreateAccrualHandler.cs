using Accounting.Application.Accruals.Create;
using Accounting.Application.Accruals.Exceptions;
using Accounting.Application.Accruals.Queries;

namespace Accounting.Application.Accruals.Handlers;

public class CreateAccrualHandler(IRepository<Accrual> repository)
    : IRequestHandler<CreateAccrualCommand, CreateAccrualResponse>
{
    public async Task<CreateAccrualResponse> Handle(CreateAccrualCommand request, CancellationToken cancellationToken)
    {
        var accrualNumber = request.AccrualNumber?.Trim() ?? string.Empty;

        // Duplicate accrual number
        var existing = await repository.FirstOrDefaultAsync(new AccrualByNumberSpec(accrualNumber), cancellationToken);
        if (existing != null)
            throw new AccrualAlreadyExistsException(accrualNumber);

        var accrual = Accrual.Create(accrualNumber, request.AccrualDate, request.Amount, request.Description);
        await repository.AddAsync(accrual, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateAccrualResponse(accrual.Id);
    }
}
