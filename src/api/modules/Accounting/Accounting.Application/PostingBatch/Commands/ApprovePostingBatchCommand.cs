namespace Accounting.Application.PostingBatch.Commands
{
    public class ApprovePostingBatchCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
        public string ApprovedBy { get; set; } = default!;
    }
}

