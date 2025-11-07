using Microsoft.Extensions.Logging;

namespace Accounting.Application.RetainedEarnings.RecordDistribution.v1;

public sealed class RecordDistributionHandler : IRequestHandler<RecordDistributionCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.RetainedEarnings> _repository;
    private readonly ILogger<RecordDistributionHandler> _logger;

    public RecordDistributionHandler(IRepository<Domain.Entities.RetainedEarnings> repository, ILogger<RecordDistributionHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DefaultIdType> Handle(RecordDistributionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording distribution for retained earnings {Id}: {Amount} on {Date}", 
            request.Id, request.Amount, request.DistributionDate);

        var re = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (re == null) throw new NotFoundException($"Retained earnings with ID {request.Id} not found");

        re.RecordDistribution(request.Amount, request.DistributionDate, request.DistributionType);
        await _repository.UpdateAsync(re, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Distribution recorded for FY{FiscalYear}", re.FiscalYear);
        return re.Id;
    }
}

