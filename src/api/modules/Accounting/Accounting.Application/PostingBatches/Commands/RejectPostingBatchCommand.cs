namespace Accounting.Application.PostingBatches.Commands;

public class RejectPostingBatchCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string RejectedBy { get; set; } = null!;
}