namespace Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

public sealed class RecordWriteOffHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<RecordWriteOffHandler> logger)
    : IRequestHandler<RecordWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordWriteOffCommand request, CancellationToken cancellationToken)
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

