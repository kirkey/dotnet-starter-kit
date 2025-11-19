namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a deduction configuration for employee payroll.
/// Defines how deductions are calculated and applied per employee or department.
/// Philippines Labor Code: Authorized deductions per Articles 111-113.
/// </summary>
public class PayrollDeduction : AuditableEntity, IAggregateRoot
{
    private PayrollDeduction() { }

    private PayrollDeduction(
        DefaultIdType id,
        DefaultIdType? payComponentId,
        string deductionType,
        decimal deductionAmount = 0,
        decimal deductionPercentage = 0)
    {
        Id = id;
        PayComponentId = payComponentId;
        DeductionType = deductionType;
        DeductionAmount = deductionAmount;
        DeductionPercentage = deductionPercentage;
        IsActive = true;
        IsAuthorized = false;
        IsRecoverable = false;
    }

    /// <summary>
    /// The pay component this deduction uses (e.g., Health Insurance, Loan).
    /// Optional for statutory deductions like SSS, PhilHealth.
    /// </summary>
    public DefaultIdType? PayComponentId { get; private set; }
    public PayComponent? PayComponent { get; private set; }

    /// <summary>
    /// Deduction type (FixedAmount, Percentage, Monthly, PerPayPeriod).
    /// Philippines: Fixed monthly deductions for authorized withholdings.
    /// </summary>
    public string DeductionType { get; private set; } = default!;

    /// <summary>
    /// Fixed deduction amount (if DeductionType is FixedAmount or Monthly).
    /// </summary>
    public decimal DeductionAmount { get; private set; }

    /// <summary>
    /// Percentage deduction (if DeductionType is Percentage).
    /// </summary>
    public decimal DeductionPercentage { get; private set; }

    /// <summary>
    /// Whether this deduction is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether this deduction is authorized per Labor Code Articles 111-113.
    /// Examples: Loans, Insurance, Union Dues, Court Orders
    /// </summary>
    public bool IsAuthorized { get; private set; }

    /// <summary>
    /// Whether this deduction can be recovered in future payrolls if amount insufficient.
    /// Labor Code prohibits recovery of certain deductions.
    /// </summary>
    public bool IsRecoverable { get; private set; }

    /// <summary>
    /// Start date for this deduction.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// End date for this deduction (if applicable).
    /// E.g., loan deduction ends when loan is paid off.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Maximum deduction limit per pay period.
    /// Labor Code Art 113: Deductions limited to 70% of wages.
    /// </summary>
    public decimal? MaxDeductionLimit { get; private set; }

    /// <summary>
    /// Employee this deduction applies to (if individual).
    /// Null if applies to all or department/area.
    /// </summary>
    public DefaultIdType? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }

    /// <summary>
    /// OrganizationalUnit (Area/Department) this applies to (if group deduction).
    /// Null if applies to individual or company-wide.
    /// </summary>
    public DefaultIdType? OrganizationalUnitId { get; private set; }
    public OrganizationalUnit? OrganizationalUnit { get; private set; }

    /// <summary>
    /// Reference number for deduction (loan ID, order ID, etc).
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Remarks or notes about the deduction.
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    /// Creates a new payroll deduction.
    /// </summary>
    public static PayrollDeduction Create(
        DefaultIdType? payComponentId,
        string deductionType,
        decimal deductionAmount = 0,
        decimal deductionPercentage = 0)
    {
        if (deductionAmount < 0 || deductionPercentage < 0)
            throw new ArgumentException("Deduction amounts cannot be negative.");

        var deduction = new PayrollDeduction(
            DefaultIdType.NewGuid(),
            payComponentId,
            deductionType,
            deductionAmount,
            deductionPercentage);

        return deduction;
    }

    /// <summary>
    /// Sets employee-specific deduction.
    /// </summary>
    public PayrollDeduction SetEmployee(DefaultIdType employeeId)
    {
        EmployeeId = employeeId;
        OrganizationalUnitId = null; // Clear area deduction
        return this;
    }

    /// <summary>
    /// Sets area/department deduction (applies to all employees in area).
    /// </summary>
    public PayrollDeduction SetOrganizationalUnit(DefaultIdType organizationalUnitId)
    {
        OrganizationalUnitId = organizationalUnitId;
        EmployeeId = null; // Clear individual employee
        return this;
    }

    /// <summary>
    /// Sets deduction as authorized per Labor Code Art 111-113.
    /// Authorized deductions: loans, insurance, union dues, court orders, etc.
    /// </summary>
    public PayrollDeduction SetAsAuthorized(bool isAuthorized = true)
    {
        IsAuthorized = isAuthorized;
        return this;
    }

    /// <summary>
    /// Sets whether unpaid deduction can be recovered in next pay period.
    /// Labor Code may prohibit recovery for certain deduction types.
    /// </summary>
    public PayrollDeduction SetRecoverable(bool isRecoverable = true)
    {
        IsRecoverable = isRecoverable;
        return this;
    }

    /// <summary>
    /// Sets deduction start and end dates.
    /// </summary>
    public PayrollDeduction SetDateRange(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && startDate > endDate.Value)
            throw new ArgumentException("Start date must be before end date.");

        StartDate = startDate;
        EndDate = endDate;
        return this;
    }

    /// <summary>
    /// Sets maximum deduction limit per Labor Code Art 113 (70% of wages).
    /// </summary>
    public PayrollDeduction SetMaxDeductionLimit(decimal maxLimit)
    {
        if (maxLimit < 0)
            throw new ArgumentException("Max deduction limit cannot be negative.");

        MaxDeductionLimit = maxLimit;
        return this;
    }

    /// <summary>
    /// Sets reference number for tracking (loan ID, court order ID, etc).
    /// </summary>
    public PayrollDeduction SetReferenceNumber(string referenceNumber)
    {
        ReferenceNumber = referenceNumber;
        return this;
    }

    /// <summary>
    /// Calculates actual deduction amount for given gross pay.
    /// Respects deduction type (fixed vs percentage) and limits.
    /// </summary>
    public decimal CalculateDeductionAmount(decimal grossPay)
    {
        decimal amount = DeductionType switch
        {
            "FixedAmount" or "Monthly" => DeductionAmount,
            "Percentage" => (grossPay * DeductionPercentage) / 100m,
            _ => 0m
        };

        // Respect maximum deduction limit (70% per Labor Code Art 113)
        if (MaxDeductionLimit.HasValue && amount > MaxDeductionLimit.Value)
            amount = MaxDeductionLimit.Value;

        // Don't deduct more than gross pay
        if (amount > grossPay)
            amount = grossPay;

        return amount;
    }

    /// <summary>
    /// Updates deduction amount.
    /// </summary>
    public PayrollDeduction UpdateDeductionAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Deduction amount cannot be negative.");

        DeductionAmount = amount;
        return this;
    }

    /// <summary>
    /// Updates deduction percentage.
    /// </summary>
    public PayrollDeduction UpdateDeductionPercentage(decimal percentage)
    {
        if (percentage is < 0 or > 100)
            throw new ArgumentException("Deduction percentage must be between 0 and 100.");

        DeductionPercentage = percentage;
        return this;
    }

    /// <summary>
    /// Updates end date.
    /// </summary>
    public PayrollDeduction UpdateEndDate(DateTime? endDate)
    {
        if (endDate.HasValue && endDate.Value < StartDate)
            throw new ArgumentException("End date must be after start date.");

        EndDate = endDate;
        return this;
    }

    /// <summary>
    /// Updates remarks.
    /// </summary>
    public PayrollDeduction UpdateRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }

    /// <summary>
    /// Checks if this deduction is active on specified date.
    /// </summary>
    public bool IsActiveOnDate(DateTime date)
    {
        if (!IsActive)
            return false;

        if (date < StartDate)
            return false;

        if (EndDate.HasValue && date > EndDate.Value)
            return false;

        return true;
    }

    /// <summary>
    /// Deactivates the deduction.
    /// </summary>
    public PayrollDeduction Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the deduction.
    /// </summary>
    public PayrollDeduction Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Payroll deduction type constants.
/// </summary>
public static class PayrollDeductionType
{
    /// <summary>Fixed amount per pay period.</summary>
    public const string FixedAmount = "FixedAmount";
    
    /// <summary>Percentage of gross pay.</summary>
    public const string Percentage = "Percentage";
    
    /// <summary>Monthly fixed amount.</summary>
    public const string Monthly = "Monthly";
    
    /// <summary>Per pay period (same as FixedAmount).</summary>
    public const string PerPayPeriod = "PerPayPeriod";
}

/// <summary>
/// Philippines Labor Code: Authorized deduction types (Articles 111-113).
/// </summary>
public static class AuthorizedDeductionType
{
    /// <summary>Employee loan repayment.</summary>
    public const string EmployeeLoan = "EmployeeLoan";
    
    /// <summary>Insurance deduction (life, health, etc).</summary>
    public const string Insurance = "Insurance";
    
    /// <summary>Union dues.</summary>
    public const string UnionDues = "UnionDues";
    
    /// <summary>Court order / Garnishment.</summary>
    public const string CourtOrder = "CourtOrder";
    
    /// <summary>Employee cooperative contributions.</summary>
    public const string CooperativeContribution = "CooperativeContribution";
    
    /// <summary>Group insurance premium.</summary>
    public const string GroupInsurance = "GroupInsurance";
    
    /// <summary>Savings / Thrift program contribution.</summary>
    public const string SavingsContribution = "SavingsContribution";
    
    /// <summary>Other authorized deduction (must document).</summary>
    public const string OtherAuthorized = "OtherAuthorized";
}

