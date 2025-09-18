namespace Accounting.Application.Consumptions.Dtos;

public class ConsumptionDto : BaseDto
{
    // Provide a writable Id in the DTO to allow handler mapping when BaseDto's Id is read-only
    public new DefaultIdType Id { get; set; }

    public DefaultIdType MeterId { get; set; }
    public DateTime ReadingDate { get; set; }
    public decimal CurrentReading { get; set; }
    public decimal PreviousReading { get; set; }
    public decimal KWhUsed { get; set; }
    public string BillingPeriod { get; set; } = string.Empty;
    public string ReadingType { get; set; } = string.Empty;
    public decimal? Multiplier { get; set; }
    public bool IsValidReading { get; set; }
    public string? ReadingSource { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
