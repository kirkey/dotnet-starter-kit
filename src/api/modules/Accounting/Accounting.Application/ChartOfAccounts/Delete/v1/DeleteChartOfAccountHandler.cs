namespace Accounting.Application.ChartOfAccounts.Delete.v1;

public sealed class DeleteChartOfAccountHandler(
    ILogger<DeleteChartOfAccountHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<DeleteChartOfAccountCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteChartOfAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new ChartOfAccountNotFoundException(request.Id);

        await repository.DeleteAsync(account, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Account with id: {AccountId} successfully deleted", account.Id);

        return account.Id;
    }
}
