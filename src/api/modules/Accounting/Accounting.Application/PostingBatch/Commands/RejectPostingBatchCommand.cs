namespace Accounting.Application.PostingBatch.Commands
{
    public class RejectPostingBatchCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
        public string RejectedBy { get; set; } = default!;
    }
}

