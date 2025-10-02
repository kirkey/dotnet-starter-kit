using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

public class SearchRecurringJournalEntriesSpec : Specification<RecurringJournalEntry, RecurringJournalEntryResponse>
{
    public SearchRecurringJournalEntriesSpec(SearchRecurringJournalEntriesCommand request)
    {
        Query
            .Where(e => string.IsNullOrEmpty(request.TemplateCode) || e.TemplateCode.Contains(request.TemplateCode))
            .Where(e => string.IsNullOrEmpty(request.Frequency) || e.Frequency.ToString() == request.Frequency)
            .Where(e => string.IsNullOrEmpty(request.Status) || e.Status.ToString() == request.Status)
            .Where(e => !request.IsActive.HasValue || e.IsActive == request.IsActive.Value)
            .OrderByDescending(e => e.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        Query.Select(e => new RecurringJournalEntryResponse
        {
            Id = e.Id,
            TemplateCode = e.TemplateCode,
            Description = e.Description,
            Frequency = e.Frequency.ToString(),
            CustomIntervalDays = e.CustomIntervalDays,
            Amount = e.Amount,
            DebitAccountId = e.DebitAccountId,
            CreditAccountId = e.CreditAccountId,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            NextRunDate = e.NextRunDate,
            LastGeneratedDate = e.LastGeneratedDate,
            GeneratedCount = e.GeneratedCount,
            IsActive = e.IsActive,
            Status = e.Status.ToString(),
            ApprovedBy = e.ApprovedBy,
            ApprovedDate = e.ApprovedDate,
            Memo = e.Memo,
            Notes = e.Notes,
            CreatedOn = e.CreatedOn,
            CreatedBy = e.CreatedBy,
            LastModifiedOn = e.LastModifiedOn,
            LastModifiedBy = e.LastModifiedBy
        });
    }
}
