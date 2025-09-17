namespace Accounting.Domain
{
    /// <summary>
    /// Represents revenue that has been billed/received but not yet earned, to be recognized at a future date.
    /// </summary>
    /// <remarks>
    /// Tracks recognition date, amount, and whether revenue has been recognized. Defaults: <see cref="IsRecognized"/>
    /// is <c>false</c> on creation; <see cref="RecognizedDate"/> is null until recognized.
    /// </remarks>
    public class DeferredRevenue : AuditableEntity, IAggregateRoot
    {
        /// <summary>
        /// A unique identifier for the deferred revenue entry.
        /// </summary>
        public string DeferredRevenueNumber { get; private set; }

        /// <summary>
        /// The date on which the deferred revenue should be recognized.
        /// </summary>
        public DateTime RecognitionDate { get; private set; }

        /// <summary>
        /// The deferred amount to be recognized; must be positive.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Optional description for the deferred revenue entry. Hides the base Description.
        /// </summary>
        public new string? Description { get; private set; }

        /// <summary>
        /// Whether the deferred revenue has been recognized.
        /// </summary>
        /// <remarks>Defaults to <c>false</c> on creation; set to <c>true</c> by <see cref="Recognize"/>.</remarks>
        public bool IsRecognized { get; private set; }

        /// <summary>
        /// When the deferred revenue was recognized, if applicable.
        /// </summary>
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

        /// <summary>
        /// Factory to create a new deferred revenue entry with validation.
        /// </summary>
        public static DeferredRevenue Create(string deferredRevenueNumber, DateTime recognitionDate, decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(deferredRevenueNumber))
                throw new ArgumentException("Deferred revenue number is required.");
            if (amount <= 0)
                throw new ArgumentException("Deferred revenue amount must be positive.");
            return new DeferredRevenue(deferredRevenueNumber, recognitionDate, amount, description);
        }

        /// <summary>
        /// Mark the deferred revenue as recognized and set the recognition date.
        /// </summary>
        public void Recognize(DateTime recognizedDate)
        {
            if (IsRecognized)
                throw new InvalidOperationException("Deferred revenue already recognized.");
            IsRecognized = true;
            RecognizedDate = recognizedDate;
        }
    }
}
