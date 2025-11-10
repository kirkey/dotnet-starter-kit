namespace Accounting.Application.ChartOfAccounts.Deactivate.v1;

/// <summary>
/// Handler for deactivating a chart of account.
/// </summary>
public sealed class DeactivateChartOfAccountHandler(
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository,
    ILogger<DeactivateChartOfAccountHandler> logger)
    : IRequestHandler<DeactivateChartOfAccountCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeactivateChartOfAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Deactivating chart of account {Id}", request.Id);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            throw new ChartOfAccountNotFoundException(request.Id);

        account.Deactivate();

        await repository.UpdateAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Chart of account {AccountCode} deactivated successfully", account.AccountCode);
        return account.Id;
    }
}

