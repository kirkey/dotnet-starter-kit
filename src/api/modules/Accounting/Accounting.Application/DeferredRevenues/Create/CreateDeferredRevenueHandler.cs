using Accounting.Application.DeferredRevenues.Specs;

namespace Accounting.Application.DeferredRevenues.Create;

public sealed class CreateDeferredRevenueHandler(
    IRepository<DeferredRevenue> repository,
    ILogger<CreateDeferredRevenueHandler> logger)
    : IRequestHandler<CreateDeferredRevenueCommand, DefaultIdType>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    
    private readonly ILogger<CreateDeferredRevenueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(CreateDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating deferred revenue: {Number}", request.DeferredRevenueNumber);

        // Check for duplicate number
        var spec = new DuplicateDeferredRevenueNumberSpec(request.DeferredRevenueNumber);
        var exists = await _repository.AnyAsync(spec, cancellationToken);
        
        if (exists)
            throw new DuplicateDeferredRevenueNumberException(request.DeferredRevenueNumber);

        var deferredRevenue = DeferredRevenue.Create(
            request.DeferredRevenueNumber,
            request.RecognitionDate,
            request.Amount,
            request.Description ?? string.Empty);

        await _repository.AddAsync(deferredRevenue, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deferred revenue created: {Id}", deferredRevenue.Id);
        return deferredRevenue.Id;
    }
}

