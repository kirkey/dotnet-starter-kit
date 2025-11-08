namespace Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

/// <summary>
/// Handler for recording AR write-offs.
/// </summary>
public sealed class RecordARWriteOffHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<RecordARWriteOffHandler> logger)
    : IRequestHandler<RecordARWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordARWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordARWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording write-off for AR account {Id}: {Amount}", request.Id, request.Amount);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AR account with ID {request.Id} not found");

        account.RecordWriteOff(request.Amount);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off recorded for {AccountNumber}: YTD={YTD}", 
            account.AccountNumber, account.YearToDateWriteOffs);
        return account.Id;
    }
}

