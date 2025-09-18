namespace Accounting.Application.PostingBatches.Commands;

public class PostingBatchCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}