using Accounting.Application.Accruals.Commands;
using Accounting.Application.Accruals.Queries;
using Accounting.Application.Accruals.Exceptions;

namespace Accounting.Application.Accruals.Handlers
{
    public class CreateAccrualHandler(IRepository<Accounting.Domain.Accrual> repository)
        : IRequestHandler<CreateAccrualCommand, DefaultIdType>
    {
        public async Task<DefaultIdType> Handle(CreateAccrualCommand request, CancellationToken cancellationToken)
        {
            var accrualNumber = request.AccrualNumber?.Trim() ?? string.Empty;

            // Duplicate accrual number
            var existing = await repository.FirstOrDefaultAsync(new AccrualByNumberSpec(accrualNumber), cancellationToken);
            if (existing != null)
                throw new AccrualAlreadyExistsException(accrualNumber);

            var accrual = Accounting.Domain.Accrual.Create(accrualNumber, request.AccrualDate, request.Amount, request.Description);
            await repository.AddAsync(accrual, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return accrual.Id;
        }
    }
}
