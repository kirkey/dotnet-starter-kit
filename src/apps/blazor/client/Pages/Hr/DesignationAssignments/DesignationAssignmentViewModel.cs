namespace FSH.Starter.Blazor.Client.Pages.Hr.DesignationAssignments;

/// <summary>
/// View model for DesignationAssignment CRUD operations.
/// Supports both Plantilla (primary) and Acting As (temporary) assignments.
/// </summary>
public class DesignationAssignmentViewModel
{
    /// <summary>
    /// The unique identifier for this assignment.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The employee being assigned to a designation.
    /// </summary>
    public DefaultIdType EmployeeId { get; set; }

    /// <summary>
    /// String representation of EmployeeId for UI binding.
    /// </summary>
    public string? EmployeeIdString
    {
        get => EmployeeId == DefaultIdType.Empty ? null : EmployeeId.ToString();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                EmployeeId = DefaultIdType.Empty;
            else if (DefaultIdType.TryParse(value, out var id))
                EmployeeId = id;
        }
    }

    public string? EmployeeNumber { get; set; }
    public string? EmployeeName { get; set; }

    /// <summary>
    /// The designation being assigned.
    /// </summary>
    public DefaultIdType DesignationId { get; set; }

    /// <summary>
    /// String representation of DesignationId for UI binding.
    /// </summary>
    public string? DesignationIdString
    {
        get => DesignationId == DefaultIdType.Empty ? null : DesignationId.ToString();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                DesignationId = DefaultIdType.Empty;
            else if (DefaultIdType.TryParse(value, out var id))
                DesignationId = id;
        }
    }

    public string? DesignationTitle { get; set; }

    /// <summary>
    /// When this assignment becomes effective.
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// When this assignment ends (for Acting As or temporary assignments).
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Whether this is the primary/plantilla designation.
    /// Only one plantilla per employee at a time.
    /// </summary>
    public bool IsPlantilla { get; set; } = true;

    /// <summary>
    /// Whether this is a temporary "Acting As" designation.
    /// </summary>
    public bool IsActingAs { get; set; }

    /// <summary>
    /// Optional salary adjustment for this assignment.
    /// Used when acting designation has different pay.
    /// </summary>
    public decimal? AdjustedSalary { get; set; }

    /// <summary>
    /// Reason for the assignment (promotion, acting, temporary, etc.).
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Whether this assignment is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Tenure in months for this assignment.
    /// </summary>
    public int TenureMonths { get; set; }

    /// <summary>
    /// Tenure display (e.g., "2 years 3 months").
    /// </summary>
    public string? TenureDisplay { get; set; }

    /// <summary>
    /// Whether this assignment is currently effective.
    /// </summary>
    public bool IsCurrentlyActive { get; set; }

    /// <summary>
    /// Assignment type for display ("Plantilla", "Acting As").
    /// </summary>
    public string? AssignmentType
    {
        get => IsPlantilla ? "Plantilla" : IsActingAs ? "Acting As" : "Other";
        set => _ = value; // Allows UI binding
    }

    /// <summary>
    /// Indicates if this is a new assignment (for create operations).
    /// </summary>
    public bool IsNew => Id == DefaultIdType.Empty;
}

