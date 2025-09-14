namespace Accounting.Application.ConsumptionData.Commands;

public class UpdateConsumptionDataCommand : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public decimal? CurrentReading { get; set; }
    public decimal? PreviousReading { get; set; }
    public string? ReadingType { get; set; }
    public decimal? Multiplier { get; set; }
    public string? ReadingSource { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

