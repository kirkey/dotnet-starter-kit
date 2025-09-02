using MediatR;
using Accounting.Application.DeferredRevenue.Commands;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.DeferredRevenue.Handlers
{
    public class RecognizeDeferredRevenueHandler(IRepository<Accounting.Domain.DeferredRevenue> repository)
        : IRequestHandler<RecognizeDeferredRevenueCommand>
    {
        public async Task Handle(RecognizeDeferredRevenueCommand request, CancellationToken cancellationToken)
        {
            var deferred = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (deferred == null)
                throw new NotFoundException($"DeferredRevenue with Id {request.Id} not found");
            deferred.Recognize(request.RecognizedDate);
            await repository.UpdateAsync(deferred, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
