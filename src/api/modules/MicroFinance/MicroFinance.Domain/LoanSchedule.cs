using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for LoanSchedule entity.
/// </summary>
public static class LoanScheduleConstants
{
    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;
}

/// <summary>
/// Represents a single installment in a loan's repayment schedule.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define expected payment amounts and due dates for each installment.
/// - Track payment status per installment (pending vs. paid).
/// - Calculate arrears and overdue amounts.
/// - Generate payment reminder notifications.
/// - Provide amortization schedule to borrowers.
/// 
/// Default values and constraints:
/// - LoanId: required, must reference an active loan
/// - InstallmentNumber: required, sequential number starting from 1
/// - DueDate: required, the date payment is expected
/// - PrincipalAmount: required, principal portion of this installment
/// - InterestAmount: required, interest portion of this installment
/// - TotalAmount: calculated as PrincipalAmount + InterestAmount
/// - PaidAmount: 0 by default, accumulates as payments received
/// - IsPaid: false by default, becomes true when PaidAmount >= TotalAmount
/// 
/// Business rules:
/// - Schedule is generated at disbursement based on loan terms.
/// - Interest calculation method (Flat, Declining, Equal Principal) affects amounts.
/// - Grace period delays first installment due date.
/// - Past-due unpaid installments trigger collection actions.
/// - Partial payments are allowed and tracked.
/// - Schedule recalculation required after restructuring.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="LoanRepayment"/>
public class LoanSchedule : AuditableEntity, IAggregateRoot
{
    /// <summary>Gets the loan ID this schedule belongs to.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the installment number.</summary>
    public int InstallmentNumber { get; private set; }

    /// <summary>Gets the due date for this installment.</summary>
    public DateOnly DueDate { get; private set; }

    /// <summary>Gets the principal amount due.</summary>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>Gets the interest amount due.</summary>
    public decimal InterestAmount { get; private set; }

    /// <summary>Gets the total amount due for this installment.</summary>
    public decimal TotalAmount => PrincipalAmount + InterestAmount;

    /// <summary>Gets the amount paid for this installment.</summary>
    public decimal PaidAmount { get; private set; }

    /// <summary>Gets whether this installment is fully paid.</summary>
    public bool IsPaid { get; private set; }

    /// <summary>Gets the date payment was completed.</summary>
    public DateOnly? PaidDate { get; private set; }

    private LoanSchedule() { }

    private LoanSchedule(
        DefaultIdType id,
        DefaultIdType loanId,
        int installmentNumber,
        DateOnly dueDate,
        decimal principalAmount,
        decimal interestAmount)
    {
        Id = id;
        LoanId = loanId;
        InstallmentNumber = installmentNumber;
        DueDate = dueDate;
        PrincipalAmount = principalAmount;
        InterestAmount = interestAmount;
        PaidAmount = 0;
        IsPaid = false;

        QueueDomainEvent(new LoanScheduleCreated { LoanSchedule = this });
    }

    /// <summary>
    /// Creates a new LoanSchedule instance.
    /// </summary>
    public static LoanSchedule Create(
        DefaultIdType loanId,
        int installmentNumber,
        DateOnly dueDate,
        decimal principalAmount,
        decimal interestAmount)
    {
        return new LoanSchedule(
            DefaultIdType.NewGuid(),
            loanId,
            installmentNumber,
            dueDate,
            principalAmount,
            interestAmount);
    }

    /// <summary>
    /// Applies a payment to this schedule entry.
    /// </summary>
    public LoanSchedule ApplyPayment(decimal amount, DateOnly paymentDate)
    {
        if (IsPaid)
            throw new InvalidOperationException("Schedule is already fully paid.");

        PaidAmount += amount;

        if (PaidAmount >= TotalAmount)
        {
            IsPaid = true;
            PaidDate = paymentDate;
            QueueDomainEvent(new LoanSchedulePaid { ScheduleId = Id });
        }

        return this;
    }
}
