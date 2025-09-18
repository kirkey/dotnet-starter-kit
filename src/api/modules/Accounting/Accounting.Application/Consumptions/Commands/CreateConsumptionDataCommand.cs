namespace Accounting.Application.Consumptions.Commands;

public class CreateConsumptionDataCommand : IRequest<DefaultIdType>
{
    public DefaultIdType MeterId { get; set; }
    public DateTime ReadingDate { get; set; }
    public decimal CurrentReading { get; set; }
    public decimal PreviousReading { get; set; }
    public string BillingPeriod { get; set; } = string.Empty;
    public string ReadingType { get; set; } = "Actual";
    public decimal? Multiplier { get; set; }
    public string? ReadingSource { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

