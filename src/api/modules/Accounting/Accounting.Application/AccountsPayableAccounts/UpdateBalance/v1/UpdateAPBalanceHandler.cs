namespace Accounting.Application.AccountsPayableAccounts.UpdateBalance.v1;

public sealed class UpdateAPBalanceHandler(
    IRepository<AccountsPayableAccount> repository,
    ILogger<UpdateAPBalanceHandler> logger)
    : IRequestHandler<UpdateAPBalanceCommand, DefaultIdType>
{
    private readonly IRepository<AccountsPayableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateAPBalanceHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateAPBalanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating AP balance for account {Id}", request.Id);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AP account with ID {request.Id} not found");

        account.UpdateBalance(request.Current0to30, request.Days31to60, request.Days61to90, request.Over90Days);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AP balance updated for {AccountNumber}: Total={CurrentBalance}", 
            account.AccountNumber, account.CurrentBalance);
        return account.Id;
    }
}

