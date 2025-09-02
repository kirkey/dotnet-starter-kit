using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain
{
    public class DeferredRevenue : AuditableEntity, IAggregateRoot
    {
        public string DeferredRevenueNumber { get; private set; }
        public DateTime RecognitionDate { get; private set; }
        public decimal Amount { get; private set; }
        public new string? Description { get; private set; }
        public bool IsRecognized { get; private set; }
        public DateTime? RecognizedDate { get; private set; }

        private DeferredRevenue() {
            DeferredRevenueNumber = string.Empty;
            Description = string.Empty;
        }

        private DeferredRevenue(string deferredRevenueNumber, DateTime recognitionDate, decimal amount, string description)
        {
            DeferredRevenueNumber = deferredRevenueNumber.Trim();
            RecognitionDate = recognitionDate;
            Amount = amount;
            Description = description.Trim();
            IsRecognized = false;
        }

        public static DeferredRevenue Create(string deferredRevenueNumber, DateTime recognitionDate, decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(deferredRevenueNumber))
                throw new ArgumentException("Deferred revenue number is required.");
            if (amount <= 0)
                throw new ArgumentException("Deferred revenue amount must be positive.");
            return new DeferredRevenue(deferredRevenueNumber, recognitionDate, amount, description);
        }

        public void Recognize(DateTime recognizedDate)
        {
            if (IsRecognized)
                throw new InvalidOperationException("Deferred revenue already recognized.");
            IsRecognized = true;
            RecognizedDate = recognizedDate;
        }
    }
}
