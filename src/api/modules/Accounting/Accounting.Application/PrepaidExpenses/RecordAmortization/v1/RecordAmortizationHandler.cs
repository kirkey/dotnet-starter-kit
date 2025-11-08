using Microsoft.Extensions.Logging;

namespace Accounting.Application.PrepaidExpenses.RecordAmortization.v1;

public sealed class RecordAmortizationHandler : IRequestHandler<RecordAmortizationCommand, DefaultIdType>
{
    private readonly IRepository<PrepaidExpense> _repository;
    private readonly ILogger<RecordAmortizationHandler> _logger;

    public RecordAmortizationHandler(IRepository<PrepaidExpense> repository, ILogger<RecordAmortizationHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DefaultIdType> Handle(RecordAmortizationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording amortization for prepaid expense {Id}: {Amount} on {Date}", 
            request.Id, request.AmortizationAmount, request.PostingDate);

        var prepaidExpense = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (prepaidExpense == null) throw new NotFoundException($"Prepaid expense with ID {request.Id} not found");

        prepaidExpense.RecordAmortization(request.AmortizationAmount, request.PostingDate, request.JournalEntryId);
        await _repository.UpdateAsync(prepaidExpense, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Amortization recorded for {PrepaidNumber}: Remaining={Remaining}", 
            prepaidExpense.PrepaidNumber, prepaidExpense.RemainingAmount);
        return prepaidExpense.Id;
    }
}

