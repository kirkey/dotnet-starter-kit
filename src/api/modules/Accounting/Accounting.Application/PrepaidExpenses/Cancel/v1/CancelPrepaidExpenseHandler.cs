namespace Accounting.Application.PrepaidExpenses.Cancel.v1;

public sealed class CancelPrepaidExpenseHandler(
    IRepository<PrepaidExpense> repository,
    ILogger<CancelPrepaidExpenseHandler> logger)
    : IRequestHandler<CancelPrepaidExpenseCommand, DefaultIdType>
{
    private readonly IRepository<PrepaidExpense> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<CancelPrepaidExpenseHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(CancelPrepaidExpenseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Cancelling prepaid expense {Id} - Reason: {Reason}", request.Id, request.Reason);

        var prepaidExpense = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (prepaidExpense == null) throw new NotFoundException($"Prepaid expense with ID {request.Id} not found");

        prepaidExpense.Cancel(request.Reason);
        await _repository.UpdateAsync(prepaidExpense, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Prepaid expense {PrepaidNumber} cancelled successfully", prepaidExpense.PrepaidNumber);
        return prepaidExpense.Id;
    }
}
