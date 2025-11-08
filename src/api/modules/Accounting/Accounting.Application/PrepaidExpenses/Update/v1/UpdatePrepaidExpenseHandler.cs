namespace Accounting.Application.PrepaidExpenses.Update.v1;

public sealed class UpdatePrepaidExpenseHandler(
    IRepository<PrepaidExpense> repository,
    ILogger<UpdatePrepaidExpenseHandler> logger)
    : IRequestHandler<UpdatePrepaidExpenseCommand, DefaultIdType>
{
    private readonly IRepository<PrepaidExpense> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdatePrepaidExpenseHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdatePrepaidExpenseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating prepaid expense {Id}", request.Id);

        var prepaidExpense = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (prepaidExpense == null) throw new NotFoundException($"Prepaid expense with ID {request.Id} not found");

        prepaidExpense.Update(request.Description, request.EndDate, request.CostCenterId, request.Notes);
        await _repository.UpdateAsync(prepaidExpense, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Prepaid expense {PrepaidNumber} updated successfully", prepaidExpense.PrepaidNumber);
        return prepaidExpense.Id;
    }
}

