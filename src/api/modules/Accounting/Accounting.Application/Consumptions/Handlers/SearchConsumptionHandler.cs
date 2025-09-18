using Accounting.Application.Consumptions.Dtos;
using Accounting.Application.Consumptions.Queries;

namespace Accounting.Application.Consumptions.Handlers;

public class SearchConsumptionHandler(IReadRepository<Consumption> repository)
    : IRequestHandler<SearchConsumptionQuery, List<ConsumptionDto>>
{
    public async Task<List<ConsumptionDto>> Handle(SearchConsumptionQuery request, CancellationToken cancellationToken)
    {
        var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

        if (request.MeterId.HasValue)
            query = query.Where(x => x.MeterId == request.MeterId.Value);

        if (!string.IsNullOrWhiteSpace(request.BillingPeriod))
            query = query.Where(x => x.BillingPeriod.Contains(request.BillingPeriod));

        if (request.FromDate.HasValue)
            query = query.Where(x => x.ReadingDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.ReadingDate <= request.ToDate.Value);

        if (request.Skip.HasValue)
            query = query.Skip(request.Skip.Value);
        if (request.Take.HasValue)
            query = query.Take(request.Take.Value);

        return query.Select(entity => new ConsumptionDto
        {
            Id = entity.Id,
            MeterId = entity.MeterId,
            ReadingDate = entity.ReadingDate,
            CurrentReading = entity.CurrentReading,
            PreviousReading = entity.PreviousReading,
            KWhUsed = entity.KWhUsed,
            BillingPeriod = entity.BillingPeriod,
            ReadingType = entity.ReadingType,
            Multiplier = entity.Multiplier,
            IsValidReading = entity.IsValidReading,
            ReadingSource = entity.ReadingSource,
            Description = entity.Description,
            Notes = entity.Notes
        }).ToList();
    }
}

