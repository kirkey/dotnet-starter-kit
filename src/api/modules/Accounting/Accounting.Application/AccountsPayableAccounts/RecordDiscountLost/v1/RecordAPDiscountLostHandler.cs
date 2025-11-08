namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

/// <summary>
/// Handler for recording AP discount lost.
/// </summary>
public sealed class RecordAPDiscountLostHandler(
    IRepository<AccountsPayableAccount> repository,
    ILogger<RecordAPDiscountLostHandler> logger)
    : IRequestHandler<RecordAPDiscountLostCommand, DefaultIdType>
{
    private readonly IRepository<AccountsPayableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordAPDiscountLostHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordAPDiscountLostCommand request, CancellationToken cancellationToken)
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

