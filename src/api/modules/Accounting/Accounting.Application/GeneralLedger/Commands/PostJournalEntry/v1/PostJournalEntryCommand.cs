namespace Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;

public class PostJournalEntryCommand : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType JournalEntryId { get; set; }
    public DateTime PostingDate { get; set; }
    public string? PostingReference { get; set; }
    public string? PostingNotes { get; set; }
    public bool ValidateBalances { get; set; } = true;
}
