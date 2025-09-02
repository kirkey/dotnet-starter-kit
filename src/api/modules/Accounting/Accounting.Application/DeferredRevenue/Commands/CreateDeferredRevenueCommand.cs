using MediatR;

namespace Accounting.Application.DeferredRevenue.Commands
{
    public class CreateDeferredRevenueCommand : IRequest<DefaultIdType>
    {
        public string DeferredRevenueNumber { get; set; } = default!;
        public DateTime RecognitionDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
    }
}

