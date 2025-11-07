namespace Accounting.Application.AccountsPayableAccounts.RecordPayment.v1;

public sealed class RecordAPPaymentHandler(
    IRepository<AccountsPayableAccount> repository,
    ILogger<RecordAPPaymentHandler> logger)
    : IRequestHandler<RecordAPPaymentCommand, DefaultIdType>
{
    private readonly IRepository<AccountsPayableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordAPPaymentHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordAPPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording payment for AP account {Id}: {Amount}", request.Id, request.Amount);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AP account with ID {request.Id} not found");

        account.RecordPayment(request.Amount, request.DiscountTaken, request.DiscountAmount);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment recorded for {AccountNumber}: YTD={YTD}", 
            account.AccountNumber, account.YearToDatePayments);
        return account.Id;
    }
}

