using Accounting.Application.DeferredRevenues.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class CreateDeferredRevenueHandler(IRepository<DeferredRevenue> repository)
    : IRequestHandler<CreateDeferredRevenueCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        var deferred = DeferredRevenue.Create(request.DeferredRevenueNumber, request.RecognitionDate, request.Amount, request.Description);
        await repository.AddAsync(deferred, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return deferred.Id;
    }
}
