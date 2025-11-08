namespace Accounting.Application.PrepaidExpenses.Close.v1;

public sealed class ClosePrepaidExpenseHandler(
    IRepository<PrepaidExpense> repository,
    ILogger<ClosePrepaidExpenseHandler> logger)
    : IRequestHandler<ClosePrepaidExpenseCommand, DefaultIdType>
{
    private readonly IRepository<PrepaidExpense> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ClosePrepaidExpenseHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ClosePrepaidExpenseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Closing prepaid expense {Id}", request.Id);

        var prepaidExpense = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (prepaidExpense == null) throw new NotFoundException($"Prepaid expense with ID {request.Id} not found");

        prepaidExpense.Close();
        await _repository.UpdateAsync(prepaidExpense, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Prepaid expense {PrepaidNumber} closed successfully", prepaidExpense.PrepaidNumber);
        return prepaidExpense.Id;
    }
}

