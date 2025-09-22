using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Queries;

public sealed record GetAccrualByIdQuery(DefaultIdType Id) : IRequest<AccrualResponse>;

