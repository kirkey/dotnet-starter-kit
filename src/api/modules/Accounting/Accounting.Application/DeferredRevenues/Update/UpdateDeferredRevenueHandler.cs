namespace Accounting.Application.DeferredRevenues.Update;

public sealed class UpdateDeferredRevenueHandler(
    IRepository<DeferredRevenue> repository,
    ILogger<UpdateDeferredRevenueHandler> logger)
    : IRequestHandler<UpdateDeferredRevenueCommand, DefaultIdType>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateDeferredRevenueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating deferred revenue: {Id}", request.Id);

        var deferredRevenue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferredRevenue == null)
            throw new DeferredRevenueByIdNotFoundException(request.Id);

        // Prevent updates to recognized revenue
        if (deferredRevenue.IsRecognized)
            throw new InvalidOperationException("Cannot update recognized deferred revenue.");

        // Use reflection to update properties (simple approach for optional parameters)
        if (request.RecognitionDate.HasValue)
        {
            var propRecognitionDate = typeof(DeferredRevenue).GetProperty(nameof(deferredRevenue.RecognitionDate));
            propRecognitionDate?.SetValue(deferredRevenue, request.RecognitionDate.Value);
        }

        if (request.Amount.HasValue)
        {
            if (request.Amount.Value <= 0)
                throw new InvalidDeferredRevenueAmountException();
            
            var propAmount = typeof(DeferredRevenue).GetProperty(nameof(deferredRevenue.Amount));
            propAmount?.SetValue(deferredRevenue, request.Amount.Value);
        }

        if (request.Description != null)
        {
            var propDescription = typeof(DeferredRevenue).GetProperty(nameof(deferredRevenue.Description));
            propDescription?.SetValue(deferredRevenue, request.Description);
        }

        await _repository.UpdateAsync(deferredRevenue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deferred revenue updated: {Id}", deferredRevenue.Id);
        return deferredRevenue.Id;
    }
}
