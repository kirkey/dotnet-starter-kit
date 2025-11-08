namespace Accounting.Application.PrepaidExpenses.RecordAmortization.v1;

public sealed record RecordAmortizationCommand(
    DefaultIdType Id,
    decimal AmortizationAmount,
    DateTime PostingDate,
    DefaultIdType? JournalEntryId = null
) : IRequest<DefaultIdType>;

