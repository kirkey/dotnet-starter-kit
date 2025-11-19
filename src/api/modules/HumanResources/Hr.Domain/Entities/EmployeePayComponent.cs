namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

using Events;

/// <summary>
/// Represents an employee-specific pay component assignment.
/// Allows per-employee overrides and custom additions to standard pay components.
/// </summary>
/// <remarks>
/// Examples:
/// - Employee gets special allowance (e.g., transportation allowance ₱2,000/month)
/// - Employee has custom loan deduction (e.g., cooperative loan ₱5,000/month)
/// - Employee gets location differential (e.g., remote area allowance ₱3,000/month)
/// - Override SSS contribution (e.g., voluntary contribution)
/// </remarks>
public class EmployeePayComponent : AuditableEntity, IAggregateRoot
{
    private EmployeePayComponent() { }

    private EmployeePayComponent(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType payComponentId,
        string assignmentType)
    {
        Id = id;
        EmployeeId = employeeId;
        PayComponentId = payComponentId;
        AssignmentType = assignmentType;
        IsActive = true;
        IsRecurring = true;
    }

    /// <summary>
    /// The employee this pay component is assigned to.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The pay component being assigned.
    /// </summary>
    public DefaultIdType PayComponentId { get; private set; }
    public PayComponent PayComponent { get; private set; } = default!;

    /// <summary>
    /// Assignment type: Standard, Override, Addition, OneTime.
    /// </summary>
    public string AssignmentType { get; private set; } = default!;

    /// <summary>
    /// Custom rate for this employee (overrides standard rate).
    /// Example: Voluntary SSS contribution at higher rate.
    /// </summary>
    public decimal? CustomRate { get; private set; }

    /// <summary>
    /// Fixed amount for this employee (overrides standard calculation).
    /// Example: Fixed transportation allowance ₱2,000/month.
    /// </summary>
    public decimal? FixedAmount { get; private set; }

    /// <summary>
    /// Custom formula for this employee (overrides standard formula).
    /// </summary>
    public string? CustomFormula { get; private set; }

    /// <summary>
    /// Effective start date for this assignment.
    /// </summary>
    public DateTime EffectiveStartDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Effective end date for this assignment (null = ongoing).
    /// </summary>
    public DateTime? EffectiveEndDate { get; private set; }

    /// <summary>
    /// Whether this assignment is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether this is a recurring assignment (appears every payroll).
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Whether this is a one-time payment/deduction.
    /// </summary>
    public bool IsOneTime { get; private set; }

    /// <summary>
    /// Date for one-time payment/deduction.
    /// </summary>
    public DateTime? OneTimeDate { get; private set; }

    /// <summary>
    /// Number of installments (for loan deductions).
    /// </summary>
    public int? InstallmentCount { get; private set; }

    /// <summary>
    /// Current installment number (for tracking loan payments).
    /// </summary>
    public int? CurrentInstallment { get; private set; }

    /// <summary>
    /// Total amount (for loans or installment-based deductions).
    /// </summary>
    public decimal? TotalAmount { get; private set; }

    /// <summary>
    /// Remaining balance (for loans).
    /// </summary>
    public decimal? RemainingBalance { get; private set; }

    /// <summary>
    /// Reference number (e.g., loan ID, order ID).
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Approver (manager or HR who approved this assignment).
    /// </summary>
    public DefaultIdType? ApprovedBy { get; private set; }

    /// <summary>
    /// Approval date.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Remarks or notes about this assignment.
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    /// Creates a fixed amount assignment.
    /// </summary>
    public static EmployeePayComponent CreateFixedAmount(
        DefaultIdType employeeId,
        DefaultIdType payComponentId,
        decimal fixedAmount,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null,
        string? referenceNumber = null)
    {
        var assignment = new EmployeePayComponent(
            DefaultIdType.NewGuid(),
            employeeId,
            payComponentId,
            "Addition");

        assignment.FixedAmount = fixedAmount;
        assignment.EffectiveStartDate = effectiveStartDate;
        assignment.EffectiveEndDate = effectiveEndDate;
        assignment.ReferenceNumber = referenceNumber;

        return assignment;
    }

    /// <summary>
    /// Creates a rate override assignment.
    /// </summary>
    public static EmployeePayComponent CreateRateOverride(
        DefaultIdType employeeId,
        DefaultIdType payComponentId,
        decimal customRate,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null)
    {
        var assignment = new EmployeePayComponent(
            DefaultIdType.NewGuid(),
            employeeId,
            payComponentId,
            "Override");

        assignment.CustomRate = customRate;
        assignment.EffectiveStartDate = effectiveStartDate;
        assignment.EffectiveEndDate = effectiveEndDate;

        return assignment;
    }

    /// <summary>
    /// Creates a loan/installment deduction.
    /// </summary>
    public static EmployeePayComponent CreateLoan(
        DefaultIdType employeeId,
        DefaultIdType payComponentId,
        decimal totalAmount,
        int installmentCount,
        DateTime effectiveStartDate,
        string? referenceNumber = null)
    {
        var monthlyPayment = totalAmount / installmentCount;

        var assignment = new EmployeePayComponent(
            DefaultIdType.NewGuid(),
            employeeId,
            payComponentId,
            "Addition");

        assignment.TotalAmount = totalAmount;
        assignment.RemainingBalance = totalAmount;
        assignment.InstallmentCount = installmentCount;
        assignment.CurrentInstallment = 0;
        assignment.FixedAmount = monthlyPayment;
        assignment.EffectiveStartDate = effectiveStartDate;
        assignment.ReferenceNumber = referenceNumber;

        return assignment;
    }

    /// <summary>
    /// Creates a one-time payment/deduction.
    /// </summary>
    public static EmployeePayComponent CreateOneTime(
        DefaultIdType employeeId,
        DefaultIdType payComponentId,
        decimal amount,
        DateTime oneTimeDate,
        string? referenceNumber = null)
    {
        var assignment = new EmployeePayComponent(
            DefaultIdType.NewGuid(),
            employeeId,
            payComponentId,
            "OneTime");

        assignment.FixedAmount = amount;
        assignment.IsRecurring = false;
        assignment.IsOneTime = true;
        assignment.OneTimeDate = oneTimeDate;
        assignment.EffectiveStartDate = oneTimeDate;
        assignment.ReferenceNumber = referenceNumber;

        return assignment;
    }

    /// <summary>
    /// Updates assignment details.
    /// </summary>
    public EmployeePayComponent Update(
        decimal? customRate = null,
        decimal? fixedAmount = null,
        string? customFormula = null,
        string? remarks = null)
    {
        if (customRate.HasValue)
            CustomRate = customRate.Value;

        if (fixedAmount.HasValue)
            FixedAmount = fixedAmount.Value;

        if (customFormula != null)
            CustomFormula = customFormula;

        if (remarks != null)
            Remarks = remarks;

        return this;
    }

    /// <summary>
    /// Records a loan payment and updates balance.
    /// </summary>
    public EmployeePayComponent RecordPayment(decimal paymentAmount)
    {
        if (!TotalAmount.HasValue || !RemainingBalance.HasValue)
            throw new InvalidOperationException("This is not a loan assignment.");

        CurrentInstallment = (CurrentInstallment ?? 0) + 1;
        RemainingBalance -= paymentAmount;

        // Deactivate if fully paid
        if (RemainingBalance <= 0)
        {
            RemainingBalance = 0;
            IsActive = false;
            EffectiveEndDate = DateTime.UtcNow;
        }

        return this;
    }

    /// <summary>
    /// Approves this assignment.
    /// </summary>
    public EmployeePayComponent Approve(DefaultIdType approvedBy)
    {
        ApprovedBy = approvedBy;
        ApprovedDate = DateTime.UtcNow;
        return this;
    }

    /// <summary>
    /// Deactivates the assignment.
    /// </summary>
    public EmployeePayComponent Deactivate()
    {
        IsActive = false;
        EffectiveEndDate = DateTime.UtcNow;
        return this;
    }

    /// <summary>
    /// Activates the assignment.
    /// </summary>
    public EmployeePayComponent Activate()
    {
        IsActive = true;
        EffectiveEndDate = null;
        return this;
    }
}

