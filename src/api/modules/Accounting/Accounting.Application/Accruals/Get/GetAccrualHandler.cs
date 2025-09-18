namespace Accounting.Application.Accruals.Get;

using Dtos;

public class GetAccrualHandler(IReadRepository<Accrual> repository)
    : IRequestHandler<GetAccrualRequest, AccrualDto>
{
    public async Task<AccrualDto> Handle(GetAccrualRequest request, CancellationToken cancellationToken)
    {
        var accrual = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (accrual == null)
            throw new NotFoundException($"Accrual with Id {request.Id} not found");
        return new AccrualDto
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

