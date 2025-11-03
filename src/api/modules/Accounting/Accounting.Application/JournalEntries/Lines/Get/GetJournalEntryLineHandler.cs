using Accounting.Application.JournalEntries.Lines.Responses;
using Accounting.Application.JournalEntries.Lines.Specs;

namespace Accounting.Application.JournalEntries.Lines.Get;


/// <summary>
/// Handler for getting a single journal entry line by ID.
/// Uses specification to include Account navigation property for AccountCode and AccountName.
/// </summary>
public sealed class GetJournalEntryLineHandler(
    [FromKeyedServices("accounting:journal-lines")] IReadRepository<JournalEntryLine> repository)
    : IRequestHandler<GetJournalEntryLineQuery, JournalEntryLineResponse>
{
    public async Task<JournalEntryLineResponse> Handle(GetJournalEntryLineQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetJournalEntryLineSpec(request.Id);
        var line = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (line == null)
            throw new JournalEntryLineNotFoundException(request.Id);

        return line.Adapt<JournalEntryLineResponse>();
    }
}

