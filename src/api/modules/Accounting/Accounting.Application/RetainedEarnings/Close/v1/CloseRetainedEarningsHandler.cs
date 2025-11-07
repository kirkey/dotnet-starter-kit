namespace Accounting.Application.RetainedEarnings.Close.v1;

public sealed class CloseRetainedEarningsHandler(
    IRepository<Domain.Entities.RetainedEarnings> repository,
    ILogger<CloseRetainedEarningsHandler> logger)
    : IRequestHandler<CloseRetainedEarningsCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.RetainedEarnings> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<CloseRetainedEarningsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(CloseRetainedEarningsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Closing retained earnings {Id}", request.Id);

        var re = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (re == null) throw new NotFoundException($"Retained earnings with ID {request.Id} not found");

        re.Close(request.ClosedBy);
        await _repository.UpdateAsync(re, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Retained earnings FY{FiscalYear} closed by {ClosedBy}", re.FiscalYear, request.ClosedBy);
        return re.Id;
    }
}

