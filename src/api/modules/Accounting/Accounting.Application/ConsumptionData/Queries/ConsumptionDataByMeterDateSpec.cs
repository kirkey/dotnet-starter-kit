namespace Accounting.Application.ConsumptionData.Queries;

public sealed class ConsumptionDataByMeterDateSpec : Specification<Accounting.Domain.ConsumptionData>, ISingleResultSpecification<Accounting.Domain.ConsumptionData>
{
    public ConsumptionDataByMeterDateSpec(DefaultIdType meterId, DateTime readingDate)
    {
        Query.Where(c => c.MeterId == meterId && c.ReadingDate == readingDate);
    }
}

