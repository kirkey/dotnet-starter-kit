namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Staff;

/// <summary>
/// ViewModel used by the Staff page for add/edit operations.
/// Mirrors the shape of the API's CreateStaffCommand and UpdateStaffCommand.
/// </summary>
public class StaffViewModel
{
    /// <summary>
    /// Primary identifier of the staff member.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique employee number. Example: "EMP-001".
    /// </summary>
    public string? EmployeeNumber { get; set; }

    /// <summary>
    /// Staff member's first name. Required.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Staff member's last name. Required.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Staff member's middle name (optional).
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Staff member's email address. Required.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Staff member's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Job title. Example: "Loan Officer", "Branch Manager".
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// Role in the MFI: "LoanOfficer", "BranchManager", "Teller", "Collector", etc.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Employment type: "FullTime", "PartTime", "Contract".
    /// </summary>
    public string? EmploymentType { get; set; }

    /// <summary>
    /// Branch assignment.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Department within the organization.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Date when the staff member joined.
    /// </summary>
    public DateOnly? JoiningDate { get; set; }

    /// <summary>
    /// DateTime wrapper for JoiningDate to work with MudDatePicker.
    /// </summary>
    public DateTime? JoiningDateDate
    {
        get => JoiningDate?.ToDateTime(TimeOnly.MinValue);
        set => JoiningDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Linked user account ID for authentication.
    /// </summary>
    public Guid? UserId { get; set; }
}
