namespace Accounting.Application.DeferredRevenues.Recognize;

public sealed class RecognizeDeferredRevenueHandler(
    IRepository<DeferredRevenue> repository,
    ILogger<RecognizeDeferredRevenueHandler> logger)
    : IRequestHandler<RecognizeDeferredRevenueCommand, DefaultIdType>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecognizeDeferredRevenueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecognizeDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Recognizing deferred revenue: {Id}", request.Id);

        var deferredRevenue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferredRevenue == null)
            throw new DeferredRevenueByIdNotFoundException(request.Id);

        deferredRevenue.Recognize(request.RecognizedDate);

        await _repository.UpdateAsync(deferredRevenue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deferred revenue recognized: {Id} on {Date}", 
            deferredRevenue.Id, request.RecognizedDate);
        
        return deferredRevenue.Id;
    }
}
