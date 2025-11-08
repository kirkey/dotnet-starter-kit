namespace Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

/// <summary>
/// Handler for recording AR collections (payments received).
/// </summary>
public sealed class RecordARCollectionHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<RecordARCollectionHandler> logger)
    : IRequestHandler<RecordARCollectionCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordARCollectionHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordARCollectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording collection for AR account {Id}: {Amount}", request.Id, request.Amount);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AR account with ID {request.Id} not found");

        account.RecordCollection(request.Amount);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Collection recorded for {AccountNumber}: YTD={YTD}", 
            account.AccountNumber, account.YearToDateCollections);
        return account.Id;
    }
}

