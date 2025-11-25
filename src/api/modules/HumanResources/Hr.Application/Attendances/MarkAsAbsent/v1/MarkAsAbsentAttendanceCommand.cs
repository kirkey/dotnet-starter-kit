namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsAbsent.v1;

/// <summary>
/// Command to mark attendance as absent.
/// </summary>
public sealed record MarkAsAbsentAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Reason = null
) : IRequest<MarkAsAbsentAttendanceResponse>;

/// <summary>
/// Response for marking attendance as absent.
/// </summary>
public sealed record MarkAsAbsentAttendanceResponse(
    DefaultIdType Id,
    string Status,
    string? Reason);

