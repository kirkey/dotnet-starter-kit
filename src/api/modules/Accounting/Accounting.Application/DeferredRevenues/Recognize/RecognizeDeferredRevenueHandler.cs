namespace Accounting.Application.DeferredRevenues.Recognize;

/// <summary>
/// Handler for recognizing deferred revenue.
/// </summary>
public sealed class RecognizeDeferredRevenueHandler(
    ILogger<RecognizeDeferredRevenueHandler> logger,
    [FromKeyedServices("accounting:deferredrevenues")] IRepository<DeferredRevenue> repository)
    : IRequestHandler<RecognizeDeferredRevenueCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RecognizeDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deferredRevenue = await repository.GetByIdAsync(request.DeferredRevenueId, cancellationToken);
        
        if (deferredRevenue == null)
        {
            throw new NotFoundException($"Deferred revenue with id {request.DeferredRevenueId} not found");
        }

        deferredRevenue.Recognize(request.RecognitionDate);

        await repository.UpdateAsync(deferredRevenue, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deferred revenue {DeferredRevenueId} recognized on {RecognitionDate}", 
            request.DeferredRevenueId, request.RecognitionDate);

        return deferredRevenue.Id;
    }
}
