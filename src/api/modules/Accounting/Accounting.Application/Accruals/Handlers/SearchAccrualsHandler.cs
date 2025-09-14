using Accounting.Application.Accruals.Queries;
using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Handlers
{
    public class SearchAccrualsHandler(IReadRepository<Accrual> repository)
        : IRequestHandler<SearchAccrualsQuery, List<AccrualDto>>
    {
        public async Task<List<AccrualDto>> Handle(SearchAccrualsQuery request, CancellationToken cancellationToken)
        {
            var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.AccrualNumber))
                query = query.Where(x => x.AccrualNumber.Contains(request.AccrualNumber));
            if (!string.IsNullOrWhiteSpace(request.Description))
                query = query.Where(x => x.Description.Contains(request.Description));
            if (request.Skip.HasValue)
                query = query.Skip(request.Skip.Value);
            if (request.Take.HasValue)
                query = query.Take(request.Take.Value);

            return query.Select(accrual => new AccrualDto
            {
                Id = accrual.Id,
                AccrualNumber = accrual.AccrualNumber,
                AccrualDate = accrual.AccrualDate,
                Amount = accrual.Amount,
                Description = accrual.Description,
                IsReversed = accrual.IsReversed,
                ReversalDate = accrual.ReversalDate
            }).ToList();
        }
    }
}

