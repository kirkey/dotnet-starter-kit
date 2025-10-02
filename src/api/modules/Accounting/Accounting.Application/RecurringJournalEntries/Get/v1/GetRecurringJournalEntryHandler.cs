using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Get.v1;

public sealed class GetRecurringJournalEntryHandler(
    IReadRepository<RecurringJournalEntry> repository)
    : IRequestHandler<GetRecurringJournalEntryRequest, RecurringJournalEntryResponse>
{
    public async Task<RecurringJournalEntryResponse> Handle(GetRecurringJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(request.Id);

        return new RecurringJournalEntryResponse
        {
            Id = entry.Id,
            TemplateCode = entry.TemplateCode,
            Description = entry.Description,
            Frequency = entry.Frequency.ToString(),
            CustomIntervalDays = entry.CustomIntervalDays,
            Amount = entry.Amount,
            DebitAccountId = entry.DebitAccountId,
            CreditAccountId = entry.CreditAccountId,
            StartDate = entry.StartDate,
            EndDate = entry.EndDate,
            NextRunDate = entry.NextRunDate,
            LastGeneratedDate = entry.LastGeneratedDate,
            GeneratedCount = entry.GeneratedCount,
            IsActive = entry.IsActive,
            Status = entry.Status.ToString(),
            ApprovedBy = entry.ApprovedBy,
            ApprovedDate = entry.ApprovedDate,
            Memo = entry.Memo,
            Notes = entry.Notes,
            CreatedOn = entry.CreatedOn,
            CreatedBy = entry.CreatedBy,
            LastModifiedOn = entry.LastModifiedOn,
            LastModifiedBy = entry.LastModifiedBy
        };
    }
}
