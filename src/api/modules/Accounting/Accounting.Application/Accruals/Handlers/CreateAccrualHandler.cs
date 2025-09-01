using Accounting.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Accruals.Commands;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.Accruals.Handlers
{
    public class CreateAccrualHandler(IRepository<Accrual> repository)
        : IRequestHandler<CreateAccrualCommand, DefaultIdType>
    {
        public async Task<DefaultIdType> Handle(CreateAccrualCommand request, CancellationToken cancellationToken)
        {
            var accrual = Accrual.Create(request.AccrualNumber, request.AccrualDate, request.Amount, request.Description);
            await repository.AddAsync(accrual, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return accrual.Id;
        }
    }
}

