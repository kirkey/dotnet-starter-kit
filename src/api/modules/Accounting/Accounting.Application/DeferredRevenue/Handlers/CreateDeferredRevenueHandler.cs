using Accounting.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.DeferredRevenue.Commands;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.DeferredRevenue.Handlers
{
    public class CreateDeferredRevenueHandler(IRepository<Accounting.Domain.DeferredRevenue> repository)
        : IRequestHandler<CreateDeferredRevenueCommand, DefaultIdType>
    {
        public async Task<DefaultIdType> Handle(CreateDeferredRevenueCommand request, CancellationToken cancellationToken)
        {
            var deferred = Accounting.Domain.DeferredRevenue.Create(request.DeferredRevenueNumber, request.RecognitionDate, request.Amount, request.Description);
            await repository.AddAsync(deferred, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return deferred.Id;
        }
    }
}
