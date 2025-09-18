namespace Accounting.Application.Accruals.Dtos;

public class AccrualDto
{
    public DefaultIdType Id { get; set; }
    public string AccrualNumber { get; set; } = default!;
    public DateTime AccrualDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = default!;
    public bool IsReversed { get; set; }
    public DateTime? ReversalDate { get; set; }
}