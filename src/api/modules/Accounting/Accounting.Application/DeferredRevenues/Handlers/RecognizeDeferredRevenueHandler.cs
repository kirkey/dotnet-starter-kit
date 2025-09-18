using Accounting.Application.DeferredRevenues.Commands;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class RecognizeDeferredRevenueHandler(IRepository<DeferredRevenue> repository)
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