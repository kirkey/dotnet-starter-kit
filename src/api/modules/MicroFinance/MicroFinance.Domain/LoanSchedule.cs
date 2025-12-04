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
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Define expected payment amounts and due dates for each installment</description></item>
///   <item><description>Track payment status per installment (pending vs. paid)</description></item>
///   <item><description>Calculate arrears and overdue amounts</description></item>
///   <item><description>Generate payment reminder notifications</description></item>
///   <item><description>Provide amortization schedule to borrowers</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// The loan schedule is generated at disbursement based on the loan terms and interest method:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Flat Rate</strong>: Equal payments throughout; more interest in later payments</description></item>
///   <item><description><strong>Declining Balance</strong>: Decreasing payments; more interest early, more principal late</description></item>
///   <item><description><strong>Equal Principal</strong>: Fixed principal portion + decreasing interest</description></item>
/// </list>
/// <para>
/// Each installment may be for different frequencies (weekly, biweekly, monthly) and
/// the schedule respects grace periods defined in the product.
/// </para>
/// <para><strong>Payment Tracking:</strong></para>
/// <list type="bullet">
///   <item><description>PaidAmount accumulates partial and full payments</description></item>
///   <item><description>IsPaid becomes true when PaidAmount ≥ TotalAmount</description></item>
///   <item><description>Past-due unpaid installments are flagged for collections</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - The loan this schedule belongs to</description></item>
///   <item><description><see cref="LoanRepayment"/> - Actual payments applied to schedules</description></item>
/// </list>
/// </remarks>
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
