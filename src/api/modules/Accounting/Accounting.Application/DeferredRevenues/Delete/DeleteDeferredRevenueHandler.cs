namespace Accounting.Application.DeferredRevenues.Delete;

public sealed class DeleteDeferredRevenueHandler(
    IRepository<DeferredRevenue> repository,
    ILogger<DeleteDeferredRevenueHandler> logger)
    : IRequestHandler<DeleteDeferredRevenueCommand, DefaultIdType>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<DeleteDeferredRevenueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(DeleteDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger.LogInformation("Deleting deferred revenue: {Id}", request.Id);

        var deferredRevenue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferredRevenue == null)
            throw new DeferredRevenueByIdNotFoundException(request.Id);

        // Prevent deletion of recognized revenue
        if (deferredRevenue.IsRecognized)
            throw new InvalidOperationException("Cannot delete recognized deferred revenue.");

        await _repository.DeleteAsync(deferredRevenue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deferred revenue deleted: {Id}", request.Id);
        return request.Id;
    }
}
