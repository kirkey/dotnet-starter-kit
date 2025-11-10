namespace Accounting.Application.ChartOfAccounts.UpdateBalance.v1;

/// <summary>
/// Handler for updating a chart of account balance.
/// </summary>
public sealed class UpdateChartOfAccountBalanceHandler(
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository,
    ILogger<UpdateChartOfAccountBalanceHandler> logger)
    : IRequestHandler<UpdateChartOfAccountBalanceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateChartOfAccountBalanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Updating balance for chart of account {Id} to {Balance}", request.Id, request.NewBalance);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            throw new ChartOfAccountNotFoundException(request.Id);

        account.UpdateBalance(request.NewBalance);

        await repository.UpdateAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Chart of account {AccountCode} balance updated to {Balance}", 
            account.AccountCode, account.Balance);
        return account.Id;
    }
}

