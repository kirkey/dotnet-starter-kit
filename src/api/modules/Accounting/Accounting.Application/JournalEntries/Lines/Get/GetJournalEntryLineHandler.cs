using Accounting.Application.JournalEntries.Lines.Responses;

namespace Accounting.Application.JournalEntries.Lines.Get;


/// <summary>
/// Handler for getting a single journal entry line by ID.
/// </summary>
public sealed class GetJournalEntryLineHandler(
    [FromKeyedServices("accounting:journal-lines")] IReadRepository<JournalEntryLine> repository)
    : IRequestHandler<GetJournalEntryLineQuery, JournalEntryLineResponse>
{
    public async Task<JournalEntryLineResponse> Handle(GetJournalEntryLineQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var line = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (line == null)
            throw new JournalEntryLineNotFoundException(request.Id);

        return line.Adapt<JournalEntryLineResponse>();
    }
}

