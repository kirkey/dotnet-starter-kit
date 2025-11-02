using Accounting.Application.JournalEntries.Lines.Responses;
using Accounting.Application.JournalEntries.Lines.Specs;

namespace Accounting.Application.JournalEntries.Lines.Search;


/// <summary>
/// Handler for searching journal entry lines by journal entry ID.
/// </summary>
public sealed class SearchJournalEntryLinesByJournalEntryIdHandler(
    [FromKeyedServices("accounting:journal-lines")] IReadRepository<JournalEntryLine> repository)
    : IRequestHandler<SearchJournalEntryLinesByJournalEntryIdQuery, List<JournalEntryLineResponse>>
{
    public async Task<List<JournalEntryLineResponse>> Handle(
        SearchJournalEntryLinesByJournalEntryIdQuery request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new JournalEntryLinesByJournalEntryIdSpec(request.JournalEntryId);
        var lines = await repository.ListAsync(spec, cancellationToken);

        return lines.Adapt<List<JournalEntryLineResponse>>();
    }
}

