namespace Accounting.Application.PostingBatch.Commands
{
    public class ReversePostingBatchCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
        public string Reason { get; set; } = default!;
    }
}

