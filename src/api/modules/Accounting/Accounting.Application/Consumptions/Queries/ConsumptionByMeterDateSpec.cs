using Accounting.Domain.Entities;

namespace Accounting.Application.Consumptions.Queries;

public sealed class ConsumptionByMeterDateSpec : Specification<Consumption>, ISingleResultSpecification<Consumption>
{
    public ConsumptionByMeterDateSpec(DefaultIdType meterId, DateTime readingDate)
    {
        Query.Where(c => c.MeterId == meterId && c.ReadingDate == readingDate);
    }
}

