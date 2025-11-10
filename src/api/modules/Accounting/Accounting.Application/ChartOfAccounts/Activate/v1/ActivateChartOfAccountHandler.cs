namespace Accounting.Application.ChartOfAccounts.Activate.v1;

/// <summary>
/// Handler for activating a chart of account.
/// </summary>
public sealed class ActivateChartOfAccountHandler(
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository,
    ILogger<ActivateChartOfAccountHandler> logger)
    : IRequestHandler<ActivateChartOfAccountCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ActivateChartOfAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Activating chart of account {Id}", request.Id);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            throw new ChartOfAccountNotFoundException(request.Id);

        account.Activate();

        await repository.UpdateAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Chart of account {AccountCode} activated successfully", account.AccountCode);
        return account.Id;
    }
}

