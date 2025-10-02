using Accounting.Application.Accruals.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Accruals.Queries;

public sealed class GetAccrualByIdHandler(IReadRepository<Accrual> repository)
    : IRequestHandler<GetAccrualByIdQuery, AccrualResponse>
{
    public async Task<AccrualResponse> Handle(GetAccrualByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct) ?? throw new NotFoundException($"accrual {request.Id} not found");
        return new AccrualResponse(entity.Id, entity.AccrualNumber, entity.AccrualDate, entity.Amount, entity.Description, entity.IsReversed, entity.ReversalDate);
    }
}

