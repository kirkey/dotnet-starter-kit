namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MemberGroups;

/// <summary>
/// ViewModel used by the MemberGroups page for add/edit operations.
/// Mirrors the shape of the API's CreateMemberGroupCommand and UpdateMemberGroupCommand.
/// </summary>
public class MemberGroupViewModel
{
    /// <summary>
    /// Primary identifier of the member group.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique group code. Example: "GRP-001".
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Group name. Required.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the group and its purpose.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date when the group was formed.
    /// </summary>
    public DateOnly? FormationDate { get; set; }

    /// <summary>
    /// DateTime wrapper for FormationDate to work with MudDatePicker.
    /// </summary>
    public DateTime? FormationDateDate
    {
        get => FormationDate?.ToDateTime(TimeOnly.MinValue);
        set => FormationDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Member ID of the group leader.
    /// </summary>
    public Guid? LeaderMemberId { get; set; }

    /// <summary>
    /// Staff ID of the assigned loan officer.
    /// </summary>
    public Guid? LoanOfficerId { get; set; }

    /// <summary>
    /// Physical location where the group meets.
    /// </summary>
    public string? MeetingLocation { get; set; }

    /// <summary>
    /// How often the group meets: "Weekly", "Biweekly", "Monthly".
    /// </summary>
    public string? MeetingFrequency { get; set; }

    /// <summary>
    /// Day of the week for meetings.
    /// </summary>
    public string? MeetingDay { get; set; }

    /// <summary>
    /// Time of the meeting.
    /// </summary>
    public TimeOnly? MeetingTime { get; set; }

    /// <summary>
    /// TimeSpan wrapper for MeetingTime to work with MudTimePicker.
    /// </summary>
    public TimeSpan? MeetingTimeSpan
    {
        get => MeetingTime?.ToTimeSpan();
        set => MeetingTime = value.HasValue ? TimeOnly.FromTimeSpan(value.Value) : null;
    }
}
