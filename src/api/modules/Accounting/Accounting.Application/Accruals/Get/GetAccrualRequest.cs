using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Get;

public class GetAccrualRequest : IRequest<AccrualDto>
{
    public DefaultIdType Id { get; set; }
}

