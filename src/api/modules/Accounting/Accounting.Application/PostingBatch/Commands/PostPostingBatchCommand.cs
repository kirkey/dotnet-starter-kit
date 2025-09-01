using MediatR;
using System;

namespace Accounting.Application.PostingBatch.Commands
{
    public class PostPostingBatchCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
    }
}

