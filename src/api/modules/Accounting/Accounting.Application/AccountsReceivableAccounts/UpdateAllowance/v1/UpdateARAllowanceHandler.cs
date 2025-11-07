namespace Accounting.Application.AccountsReceivableAccounts.UpdateAllowance.v1;

public sealed class UpdateARAllowanceHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<UpdateARAllowanceHandler> logger)
    : IRequestHandler<UpdateARAllowanceCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateARAllowanceHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateARAllowanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating allowance for AR account {Id}: {Amount}", request.Id, request.AllowanceAmount);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AR account with ID {request.Id} not found");

        account.UpdateAllowance(request.AllowanceAmount);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Allowance updated for {AccountNumber}", account.AccountNumber);
        return account.Id;
    }
}

