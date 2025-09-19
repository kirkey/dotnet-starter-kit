using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Get;

public class GetAccrualRequest : IRequest<AccrualResponse>
{
    public DefaultIdType Id { get; set; }
}

