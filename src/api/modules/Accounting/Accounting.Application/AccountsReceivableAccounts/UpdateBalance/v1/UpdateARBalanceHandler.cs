namespace Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;

public sealed class UpdateARBalanceHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<UpdateARBalanceHandler> logger)
    : IRequestHandler<UpdateARBalanceCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateARBalanceHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateARBalanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating AR balance for account {Id}", request.Id);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AR account with ID {request.Id} not found");

        account.UpdateBalance(request.Current0to30, request.Days31to60, request.Days61to90, request.Over90Days);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AR balance updated for {AccountNumber}: Total={CurrentBalance}", 
            account.AccountNumber, account.CurrentBalance);
        return account.Id;
    }
}

