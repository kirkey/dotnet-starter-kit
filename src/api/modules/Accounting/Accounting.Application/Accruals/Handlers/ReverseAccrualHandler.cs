using Accounting.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Accruals.Commands;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.Accruals.Handlers
{
    public class ReverseAccrualHandler(IRepository<Accrual> repository) : IRequestHandler<ReverseAccrualCommand>
    {
        public async Task Handle(ReverseAccrualCommand request, CancellationToken cancellationToken)
        {
            var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (accrual == null)
                throw new NotFoundException($"Accrual with Id {request.Id} not found");
            accrual.Reverse(request.ReversalDate);
            await repository.UpdateAsync(accrual, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            // No return needed for Task
        }
    }
}
