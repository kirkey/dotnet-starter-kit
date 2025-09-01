using MediatR;
using System;
using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Queries
{
    public class GetAccrualByIdQuery(DefaultIdType id) : IRequest<AccrualDto>
    {
        public DefaultIdType Id { get; set; } = id;
    }
}

