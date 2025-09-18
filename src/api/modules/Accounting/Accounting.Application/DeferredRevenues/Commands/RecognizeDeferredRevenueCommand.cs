namespace Accounting.Application.DeferredRevenues.Commands;

public class RecognizeDeferredRevenueCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public DateTime RecognizedDate { get; set; }
}