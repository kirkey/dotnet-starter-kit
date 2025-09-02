using MediatR;

namespace Accounting.Application.PostingBatch.Commands
{
    public class PostingBatchCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
    }
}

