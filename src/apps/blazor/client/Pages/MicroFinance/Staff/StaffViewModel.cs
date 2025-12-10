namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Staff;

/// <summary>
/// ViewModel used by the Staff page for add/edit operations.
/// Mirrors the shape of the API's CreateStaffCommand and UpdateStaffCommand so Mapster/Adapt can map between them.
/// </summary>
public class StaffViewModel
{
    /// <summary>
    /// Primary identifier of the staff member.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique employee number assigned by the system.
    /// Example: "EMP-001", "STF-10001".
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
    /// Staff member's email address. Required.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Staff member's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Staff member's position/role.
    /// Examples: "Teller", "Loan Officer", "Branch Manager", "Administrator".
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// ID of the branch where the staff member is assigned.
    /// </summary>
    public DefaultIdType BranchId { get; set; }

    /// <summary>
    /// Date the staff member joined the organization.
    /// </summary>
    public DateTime? DateJoined { get; set; }

    /// <summary>
    /// Current status of the staff member.
    /// Values: "Active", "OnLeave", "Suspended", "Terminated".
    /// </summary>
    public string? Status { get; set; }
}
