namespace Accounting.Application.DeferredRevenues.Commands;

public class CreateDeferredRevenueCommand : IRequest<DefaultIdType>
{
    public string DeferredRevenueNumber { get; set; } = null!;
    public DateTime RecognitionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
}