using System;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain
{
    public class Accrual : AuditableEntity, IAggregateRoot
    {
        public string AccrualNumber { get; private set; }
        public DateTime AccrualDate { get; private set; }
        public decimal Amount { get; private set; }
        public new string? Description { get; private set; }
        public bool IsReversed { get; private set; }
        public DateTime? ReversalDate { get; private set; }

        private Accrual() {
            AccrualNumber = string.Empty;
            Description = string.Empty;
        }

        private Accrual(string accrualNumber, DateTime accrualDate, decimal amount, string description)
        {
            AccrualNumber = accrualNumber.Trim();
            AccrualDate = accrualDate;
            Amount = amount;
            Description = description.Trim();
            IsReversed = false;
        }

        public static Accrual Create(string accrualNumber, DateTime accrualDate, decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(accrualNumber))
                throw new ArgumentException("Accrual number is required.");
            if (amount <= 0)
                throw new ArgumentException("Accrual amount must be positive.");
            return new Accrual(accrualNumber, accrualDate, amount, description);
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
