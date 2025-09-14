using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain
{
    public class Accrual : AuditableEntity, IAggregateRoot
    {
        private const int MaxAccrualNumberLength = 50;
        private const int MaxDescriptionLength = 200;

        public string AccrualNumber { get; private set; }
        public DateTime AccrualDate { get; private set; }
        public decimal Amount { get; private set; }
        public new string? Description { get; private set; }
        public bool IsReversed { get; private set; }
        public DateTime? ReversalDate { get; private set; }

        // Parameterless ctor for EF
        private Accrual()
        {
            AccrualNumber = string.Empty;
            Description = string.Empty;
        }

        private Accrual(string accrualNumber, DateTime accrualDate, decimal amount, string description)
        {
            var num = accrualNumber?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(num))
                throw new ArgumentException("Accrual number is required.");
            if (num.Length > MaxAccrualNumberLength)
                throw new ArgumentException($"Accrual number cannot exceed {MaxAccrualNumberLength} characters.");

            if (amount <= 0)
                throw new ArgumentException("Accrual amount must be positive.");

            AccrualNumber = num;
            AccrualDate = accrualDate;
            Amount = amount;

            var desc = description?.Trim();
            if (desc?.Length > MaxDescriptionLength)
                desc = desc.Substring(0, MaxDescriptionLength);

            Description = desc;
            IsReversed = false;
        }

        public static Accrual Create(string accrualNumber, DateTime accrualDate, decimal amount, string description)
        {
            // Domain-level validation occurs in the private constructor
            return new Accrual(accrualNumber, accrualDate, amount, description);
        }

        public Accrual Update(string? accrualNumber, DateTime? accrualDate, decimal? amount, string? description)
        {
            if (IsReversed)
                throw new InvalidOperationException("Cannot modify a reversed accrual.");

            bool isUpdated = false;

            if (!string.IsNullOrWhiteSpace(accrualNumber) && AccrualNumber != accrualNumber)
            {
                var num = accrualNumber!.Trim();
                if (num.Length > MaxAccrualNumberLength)
                    throw new ArgumentException($"Accrual number cannot exceed {MaxAccrualNumberLength} characters.");
                AccrualNumber = num;
                isUpdated = true;
            }

            if (accrualDate.HasValue && AccrualDate != accrualDate.Value)
            {
                AccrualDate = accrualDate.Value;
                isUpdated = true;
            }

            if (amount.HasValue && Amount != amount.Value)
            {
                if (amount.Value <= 0)
                    throw new ArgumentException("Accrual amount must be positive.");
                Amount = amount.Value;
                isUpdated = true;
            }

            if (description != Description)
            {
                var desc = description?.Trim();
                if (desc?.Length > MaxDescriptionLength)
                    desc = desc!.Substring(0, MaxDescriptionLength);
                Description = desc;
                isUpdated = true;
            }

            if (isUpdated)
            {
                // Optionally queue domain event here
            }

            return this;
        }

        public void Reverse(DateTime reversalDate)
        {
            if (IsReversed)
                throw new InvalidOperationException("Accrual already reversed.");
            IsReversed = true;
            ReversalDate = reversalDate;
        }
    }
}
