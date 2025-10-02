using Accounting.Application.DeferredRevenues.Commands;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class RecognizeDeferredRevenueHandler(IRepository<DeferredRevenue> repository)
    : IRequestHandler<RecognizeDeferredRevenueCommand>
{
    public async Task Handle(RecognizeDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        var deferred = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = deferred ?? throw new DeferredRevenueByIdNotFoundException(request.Id);
        deferred.Recognize(request.RecognizedDate);
        await repository.UpdateAsync(deferred, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}