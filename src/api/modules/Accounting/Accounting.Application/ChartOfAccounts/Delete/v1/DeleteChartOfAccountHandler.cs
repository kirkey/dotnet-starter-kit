using Accounting.Domain.Entities;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;
public class DeleteChartOfAccountHandler(
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

        logger.LogInformation("account with id: {AccountId} successfully deleted", account.Id);

        return account.Id;
    }
}
