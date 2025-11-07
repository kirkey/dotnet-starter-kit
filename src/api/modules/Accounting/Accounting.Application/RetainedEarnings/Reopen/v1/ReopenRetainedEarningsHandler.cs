namespace Accounting.Application.RetainedEarnings.Reopen.v1;

public sealed class ReopenRetainedEarningsHandler(
    IRepository<Domain.Entities.RetainedEarnings> repository,
    ILogger<ReopenRetainedEarningsHandler> logger)
    : IRequestHandler<ReopenRetainedEarningsCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.RetainedEarnings> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ReopenRetainedEarningsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ReopenRetainedEarningsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reopening retained earnings {Id} - Reason: {Reason}", request.Id, request.Reason);

        var re = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (re == null) throw new NotFoundException($"Retained earnings with ID {request.Id} not found");

        re.Reopen(request.Reason);
        await _repository.UpdateAsync(re, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Retained earnings FY{FiscalYear} reopened", re.FiscalYear);
        return re.Id;
    }
}
