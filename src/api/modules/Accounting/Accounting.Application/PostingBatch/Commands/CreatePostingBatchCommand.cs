using MediatR;

namespace Accounting.Application.PostingBatch.Commands
{
    public class CreatePostingBatchCommand : IRequest<DefaultIdType>
    {
        public string BatchNumber { get; set; } = default!;
        public DateTime BatchDate { get; set; }
        public string? Description { get; set; }
        public DefaultIdType? PeriodId { get; set; }
    }
}

