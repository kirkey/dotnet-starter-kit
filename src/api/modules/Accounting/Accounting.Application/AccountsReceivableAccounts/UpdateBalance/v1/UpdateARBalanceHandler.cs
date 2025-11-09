namespace Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;

public sealed class UpdateArBalanceHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<UpdateArBalanceHandler> logger)
    : IRequestHandler<UpdateArBalanceCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateArBalanceHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateArBalanceCommand request, CancellationToken cancellationToken)
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

