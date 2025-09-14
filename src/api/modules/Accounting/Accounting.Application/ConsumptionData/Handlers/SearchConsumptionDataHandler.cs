using Accounting.Application.ConsumptionData.Queries;
using Accounting.Application.ConsumptionData.Dtos;

namespace Accounting.Application.ConsumptionData.Handlers;

public class SearchConsumptionDataHandler(IReadRepository<Accounting.Domain.ConsumptionData> repository)
    : IRequestHandler<SearchConsumptionDataQuery, List<ConsumptionDataDto>>
{
    public async Task<List<ConsumptionDataDto>> Handle(SearchConsumptionDataQuery request, CancellationToken cancellationToken)
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

        return query.Select(entity => new ConsumptionDataDto
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

