using Accounting.Application.BudgetDetails.Responses;

namespace Accounting.Application.BudgetDetails.Get;

public sealed class GetBudgetDetailHandler(IReadRepository<BudgetDetail> repo)
    : IRequestHandler<GetBudgetDetailQuery, BudgetDetailResponse>
{
    public async Task<BudgetDetailResponse> Handle(GetBudgetDetailQuery request, CancellationToken ct)
    {
        var detail = await repo.GetByIdAsync(request.Id, ct) ?? throw new NotFoundException($"budget detail {request.Id} not found");
        return detail.Adapt<BudgetDetailResponse>();
    }
}
