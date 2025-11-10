using Accounting.Application.DeferredRevenues.Specs;

namespace Accounting.Application.DeferredRevenues.Create;

public sealed class CreateDeferredRevenueHandler(
    [FromKeyedServices("accounting:deferred-revenues")] IRepository<DeferredRevenue> repository,
    ILogger<CreateDeferredRevenueHandler> logger)
    : IRequestHandler<CreateDeferredRevenueCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateDeferredRevenueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating deferred revenue: {Number}", request.DeferredRevenueNumber);

        // Check for duplicate number
        var spec = new DuplicateDeferredRevenueNumberSpec(request.DeferredRevenueNumber);
        var exists = await repository.AnyAsync(spec, cancellationToken);
        
        if (exists)
            throw new DuplicateDeferredRevenueNumberException(request.DeferredRevenueNumber);

        var deferredRevenue = DeferredRevenue.Create(
            request.DeferredRevenueNumber,
            request.RecognitionDate,
            request.Amount,
            request.Description ?? string.Empty);

        await repository.AddAsync(deferredRevenue, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deferred revenue created: {Id}", deferredRevenue.Id);
        return deferredRevenue.Id;
    }
}

