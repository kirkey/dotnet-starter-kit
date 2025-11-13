using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents the assignment of a designation to an employee.
/// Tracks both primary (plantilla) and acting designations with effective dates.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Employee can have multiple designation assignments over time
/// - One primary designation at a time (IsPlantilla = true)
/// - Multiple acting designations simultaneously allowed
/// - Each assignment tracks effective and end dates for historical records
/// - Supports salary adjustments per assignment
/// 
/// Example:
/// - Employee John Doe:
///   - Primary: Supervisor (Plantilla) - Active
///   - Acting As: Senior Manager - Jan 1 to Mar 31, 2026
///   - Previous: Staff - Historical record
/// </remarks>
public class DesignationAssignment : AuditableEntity, IAggregateRoot
{
    private DesignationAssignment() { }

    private DesignationAssignment(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType designationId,
        DateTime effectiveDate,
        bool isPlantilla,
        string? reason = null)
    {
        Id = id;
        EmployeeId = employeeId;
        DesignationId = designationId;
        EffectiveDate = effectiveDate;
        IsPlantilla = isPlantilla;
        Reason = reason;
        IsActive = true;

        QueueDomainEvent(new DesignationAssignmentCreated { Assignment = this });
    }

    /// <summary>
    /// The employee assigned to this designation.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The designation assigned to the employee.
    /// </summary>
    public DefaultIdType DesignationId { get; private set; }
    public Designation Designation { get; private set; } = default!;

    /// <summary>
    /// Date when this designation assignment becomes effective.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Date when this designation assignment ends.
    /// Null indicates ongoing assignment.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Indicates if this is the primary/plantilla designation.
    /// Only one plantilla designation per employee at a time.
    /// </summary>
    public bool IsPlantilla { get; private set; }

    /// <summary>
    /// Indicates if this employee is currently "Acting As" this designation.
    /// For temporary assignments with different duties/responsibilities.
    /// </summary>
    public bool IsActingAs { get; private set; }

    /// <summary>
    /// Optional salary adjustment for this assignment.
    /// Used when acting designation has different pay.
    /// </summary>
    public decimal? AdjustedSalary { get; private set; }

    /// <summary>
    /// Reason for the assignment (promotion, acting, temporary, etc.).
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Whether this assignment is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new primary/plantilla designation assignment.
    /// </summary>
    public static DesignationAssignment CreatePlantilla(
        DefaultIdType employeeId,
        DefaultIdType designationId,
        DateTime effectiveDate,
        string? reason = null)
    {
        var assignment = new DesignationAssignment(
            DefaultIdType.NewGuid(),
            employeeId,
            designationId,
            effectiveDate,
            isPlantilla: true,
            reason);

        return assignment;
    }

    /// <summary>
    /// Creates a new "Acting As" designation assignment.
    /// </summary>
    public static DesignationAssignment CreateActingAs(
        DefaultIdType employeeId,
        DefaultIdType designationId,
        DateTime effectiveDate,
        DateTime? endDate = null,
        decimal? adjustedSalary = null,
        string? reason = null)
    {
        var assignment = new DesignationAssignment(
            DefaultIdType.NewGuid(),
            employeeId,
            designationId,
            effectiveDate,
            isPlantilla: false,
            reason)
        {
            IsActingAs = true,
            EndDate = endDate,
            AdjustedSalary = adjustedSalary
        };

        return assignment;
    }

    /// <summary>
    /// Updates the assignment with a new end date.
    /// </summary>
    public DesignationAssignment SetEndDate(DateTime endDate)
    {
        EndDate = endDate;
        QueueDomainEvent(new DesignationAssignmentEnded { AssignmentId = Id });
        return this;
    }

    /// <summary>
    /// Updates the assignment salary.
    /// </summary>
    public DesignationAssignment SetAdjustedSalary(decimal salary)
    {
        AdjustedSalary = salary;
        QueueDomainEvent(new DesignationAssignmentUpdated { Assignment = this });
        return this;
    }

    /// <summary>
    /// Deactivates this assignment.
    /// </summary>
    public DesignationAssignment Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new DesignationAssignmentDeactivated { AssignmentId = Id });
        return this;
    }

    /// <summary>
    /// Gets whether this assignment is currently effective.
    /// </summary>
    public bool IsCurrentlyEffective(DateTime? asOfDate = null)
    {
        var checkDate = asOfDate ?? DateTime.UtcNow;
        return EffectiveDate <= checkDate && (EndDate == null || EndDate > checkDate) && IsActive;
    }

    /// <summary>
    /// Gets the tenure in months for this designation.
    /// </summary>
    public int GetTenureMonths()
    {
        var endDate = EndDate ?? DateTime.UtcNow;
        return (int)Math.Round((endDate - EffectiveDate).TotalDays / 30.44);
    }

    /// <summary>
    /// Gets the tenure as a formatted string.
    /// </summary>
    public string GetTenureDisplay()
    {
        var months = GetTenureMonths();
        var years = months / 12;
        var remainingMonths = months % 12;

        if (years > 0 && remainingMonths > 0)
            return $"{years}y {remainingMonths}m";
        if (years > 0)
            return $"{years}y";
        return $"{months}m";
    }
}
