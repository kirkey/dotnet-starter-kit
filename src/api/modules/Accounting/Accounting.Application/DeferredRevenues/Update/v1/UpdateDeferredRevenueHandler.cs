namespace Accounting.Application.DeferredRevenues.Update.v1;

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
        _logger.LogInformation("Updating deferred revenue {Id}", request.Id);

        var deferredRevenue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferredRevenue == null) throw new NotFoundException($"Deferred revenue with ID {request.Id} not found");

        if (deferredRevenue.IsRecognized)
            throw new InvalidOperationException("Cannot modify recognized deferred revenue");

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(request.Description) && deferredRevenue.Description != request.Description.Trim())
        {
            // Use reflection to update Description since entity doesn't have an Update method
            var descriptionProperty = typeof(DeferredRevenue).GetProperty(nameof(DeferredRevenue.Description));
            descriptionProperty?.SetValue(deferredRevenue, request.Description.Trim());
            isUpdated = true;
        }

        if (request.RecognitionDate.HasValue && deferredRevenue.RecognitionDate != request.RecognitionDate.Value)
        {
            var recognitionDateProperty = typeof(DeferredRevenue).GetProperty(nameof(DeferredRevenue.RecognitionDate));
            recognitionDateProperty?.SetValue(deferredRevenue, request.RecognitionDate.Value);
            isUpdated = true;
        }

        if (isUpdated)
        {
            await _repository.UpdateAsync(deferredRevenue, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Deferred revenue {Number} updated successfully", deferredRevenue.DeferredRevenueNumber);
        }

        return deferredRevenue.Id;
    }
}

