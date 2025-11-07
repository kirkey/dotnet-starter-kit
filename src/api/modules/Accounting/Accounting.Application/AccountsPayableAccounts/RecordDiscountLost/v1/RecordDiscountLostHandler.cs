namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

public sealed class RecordDiscountLostHandler(
    IRepository<AccountsPayableAccount> repository,
    ILogger<RecordDiscountLostHandler> logger)
    : IRequestHandler<RecordDiscountLostCommand, DefaultIdType>
{
    private readonly IRepository<AccountsPayableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordDiscountLostHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordDiscountLostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording discount lost for AP account {Id}: {Amount}", request.Id, request.DiscountAmount);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AP account with ID {request.Id} not found");

        account.RecordDiscountLost(request.DiscountAmount);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Discount lost recorded for {AccountNumber}: YTD Lost={YTD}", 
            account.AccountNumber, account.YearToDateDiscountsLost);
        return account.Id;
    }
}

