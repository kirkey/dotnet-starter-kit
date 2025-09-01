using System;

namespace Accounting.Application.DeferredRevenue.Dtos
{
    public class DeferredRevenueDto
    {
        public DefaultIdType Id { get; set; }
        public string DeferredRevenueNumber { get; set; } = default!;
        public DateTime RecognitionDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public bool IsRecognized { get; set; }
        public DateTime? RecognizedDate { get; set; }
    }
}

