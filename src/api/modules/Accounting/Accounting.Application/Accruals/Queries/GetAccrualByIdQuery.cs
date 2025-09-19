using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Queries;

public class GetAccrualByIdQuery(DefaultIdType id) : IRequest<AccrualResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
