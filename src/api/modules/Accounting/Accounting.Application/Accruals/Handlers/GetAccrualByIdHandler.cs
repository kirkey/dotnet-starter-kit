
namespace Accounting.Application.Accruals.Handlers;

public class GetAccrualByIdHandler(IReadRepository<Accrual> repository)
    : IRequestHandler<GetAccrualByIdQuery, AccrualResponse>
{
    public async Task<AccrualResponse> Handle(GetAccrualByIdQuery request, CancellationToken cancellationToken)
    {
        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (accrual == null)
            throw new NotFoundException($"Accrual with Id {request.Id} not found");
        return new AccrualResponse
        {
            Id = accrual.Id,
            AccrualNumber = accrual.AccrualNumber,
            AccrualDate = accrual.AccrualDate,
            Amount = accrual.Amount,
            Description = accrual.Description,
            IsReversed = accrual.IsReversed,
            ReversalDate = accrual.ReversalDate
        };
    }
}
