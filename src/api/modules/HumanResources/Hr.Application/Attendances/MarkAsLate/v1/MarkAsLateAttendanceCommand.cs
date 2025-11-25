namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLate.v1;

/// <summary>
/// Command to mark attendance as late with minutes late count.
/// </summary>
public sealed record MarkAsLateAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(15)] int MinutesLate,
    [property: DefaultValue(null)] string? Reason = null
) : IRequest<MarkAsLateAttendanceResponse>;

/// <summary>
/// Response for marking attendance as late.
/// </summary>
public sealed record MarkAsLateAttendanceResponse(
    DefaultIdType Id,
    string Status,
    int? MinutesLate);

