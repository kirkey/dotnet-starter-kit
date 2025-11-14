namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents leave balance tracking for an employee.
/// Tracks accrued, taken, and available leave days by type.
/// </summary>
public class LeaveBalance : AuditableEntity, IAggregateRoot
{
    private LeaveBalance() { }

    private LeaveBalance(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType leaveTypeId,
        int year,
        decimal openingBalance = 0)
    {
        Id = id;
        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        Year = year;
        OpeningBalance = openingBalance;
        AccruedDays = 0;
        TakenDays = 0;
        CarriedOverDays = 0;
    }

    /// <summary>
    /// The employee this balance belongs to.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The leave type.
    /// </summary>
    public DefaultIdType LeaveTypeId { get; private set; }
    public LeaveType LeaveType { get; private set; } = default!;

    /// <summary>
    /// Calendar year for this balance.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Opening balance (from prior year carryover).
    /// </summary>
    public decimal OpeningBalance { get; private set; }

    /// <summary>
    /// Days accrued during the year.
    /// </summary>
    public decimal AccruedDays { get; private set; }

    /// <summary>
    /// Days carried over from previous year.
    /// </summary>
    public decimal CarriedOverDays { get; private set; }

    /// <summary>
    /// Total available days.
    /// </summary>
    public decimal AvailableDays => OpeningBalance + AccruedDays + CarriedOverDays;

    /// <summary>
    /// Days taken (approved leave requests).
    /// </summary>
    public decimal TakenDays { get; private set; }

    /// <summary>
    /// Days pending (pending leave requests).
    /// </summary>
    public decimal PendingDays { get; private set; }

    /// <summary>
    /// Remaining balance.
    /// </summary>
    public decimal RemainingDays => AvailableDays - TakenDays - PendingDays;

    /// <summary>
    /// Expiry date for carried over days (if applicable).
    /// </summary>
    public DateTime? CarryoverExpiryDate { get; private set; }

    /// <summary>
    /// Creates a new leave balance.
    /// </summary>
    public static LeaveBalance Create(
        DefaultIdType employeeId,
        DefaultIdType leaveTypeId,
        int year,
        decimal openingBalance = 0)
    {
        var balance = new LeaveBalance(
            DefaultIdType.NewGuid(),
            employeeId,
            leaveTypeId,
            year,
            openingBalance);

        return balance;
    }

    /// <summary>
    /// Adds accrued days for a period.
    /// </summary>
    public LeaveBalance AddAccrual(decimal days)
    {
        if (days < 0)
            throw new ArgumentException("Accrual days cannot be negative.", nameof(days));

        AccruedDays += days;
        return this;
    }

    /// <summary>
    /// Records leave taken.
    /// </summary>
    public LeaveBalance RecordLeave(decimal days)
    {
        if (days < 0)
            throw new ArgumentException("Leave days cannot be negative.", nameof(days));

        if (days > RemainingDays)
            throw new InvalidOperationException($"Insufficient balance. Available: {RemainingDays}, Requested: {days}");

        TakenDays += days;
        return this;
    }

    /// <summary>
    /// Adds pending leave request days.
    /// </summary>
    public LeaveBalance AddPending(decimal days)
    {
        if (days < 0)
            throw new ArgumentException("Pending days cannot be negative.", nameof(days));

        if ((TakenDays + days + PendingDays) > AvailableDays)
            throw new InvalidOperationException($"Insufficient balance. Available: {AvailableDays}, Requested: {TakenDays + days + PendingDays}");

        PendingDays += days;
        return this;
    }

    /// <summary>
    /// Removes pending leave request days.
    /// </summary>
    public LeaveBalance RemovePending(decimal days)
    {
        if (days < 0)
            throw new ArgumentException("Pending days cannot be negative.", nameof(days));

        if (days > PendingDays)
            throw new InvalidOperationException($"Cannot remove more pending days than exist. Pending: {PendingDays}, Removing: {days}");

        PendingDays -= days;
        return this;
    }

    /// <summary>
    /// Converts pending to taken when leave is approved.
    /// </summary>
    public LeaveBalance ApprovePending(decimal days)
    {
        if (days < 0)
            throw new ArgumentException("Days cannot be negative.", nameof(days));

        if (days > PendingDays)
            throw new InvalidOperationException($"Cannot approve more than pending. Pending: {PendingDays}, Approving: {days}");

        PendingDays -= days;
        TakenDays += days;
        return this;
    }

    /// <summary>
    /// Sets carried over balance from previous year.
    /// </summary>
    public LeaveBalance SetCarryover(decimal days, DateTime? expiryDate = null)
    {
        if (days < 0)
            throw new ArgumentException("Carryover days cannot be negative.", nameof(days));

        CarriedOverDays = days;
        CarryoverExpiryDate = expiryDate;
        return this;
    }
}

