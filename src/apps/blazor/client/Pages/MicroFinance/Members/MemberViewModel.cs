namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Members;

/// <summary>
/// ViewModel used by the Members page for add/edit operations.
/// Mirrors the shape of the API's CreateMemberCommand and UpdateMemberCommand so Mapster/Adapt can map between them.
/// </summary>
public class MemberViewModel
{
    /// <summary>
    /// Primary identifier of the member.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique member number assigned by the system.
    /// Example: "MBR-001", "M-10001".
    /// </summary>
    public string? MemberNumber { get; set; }

    /// <summary>
    /// Member's first name. Required.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Member's last name. Required.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Member's middle name (optional).
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Member's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Member's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Member's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// DateTime wrapper for DateOfBirth to work with MudDatePicker.
    /// </summary>
    public DateTime? DateOfBirthDate
    {
        get => DateOfBirth?.ToDateTime(TimeOnly.MinValue);
        set => DateOfBirth = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Member's gender. Values: "Male", "Female", "Other".
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Member's street address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Government-issued national ID number.
    /// </summary>
    public string? NationalId { get; set; }

    /// <summary>
    /// Member's occupation or profession.
    /// </summary>
    public string? Occupation { get; set; }

    /// <summary>
    /// Member's monthly income for credit assessment.
    /// </summary>
    public decimal? MonthlyIncome { get; set; }

    /// <summary>
    /// Date the member joined the microfinance institution.
    /// </summary>
    public DateOnly? JoinDate { get; set; }

    /// <summary>
    /// DateTime wrapper for JoinDate to work with MudDatePicker.
    /// </summary>
    public DateTime? JoinDateDate
    {
        get => JoinDate?.ToDateTime(TimeOnly.MinValue);
        set => JoinDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Additional notes or comments about the member.
    /// </summary>
    public string? Notes { get; set; }
}
