using MediatR;
using System;
using Accounting.Application.PostingBatch.Dtos;

namespace Accounting.Application.PostingBatch.Queries
{
    public class GetPostingBatchByIdQuery(DefaultIdType id) : IRequest<PostingBatchDto>
    {
        public DefaultIdType Id { get; set; } = id;
    }
}

