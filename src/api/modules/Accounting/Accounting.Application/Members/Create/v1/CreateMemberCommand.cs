namespace Accounting.Application.Members.Create.v1;

/// <summary>
/// Command to create a new member account.
/// </summary>
public sealed record CreateMemberCommand(
    string MemberNumber,
    string MemberName,
    string ServiceAddress,
    DateTime MembershipDate,
    string? MailingAddress = null,
    string? ContactInfo = null,
    string AccountStatus = "Active",
    DefaultIdType? MeterId = null,
    string? Email = null,
    string? PhoneNumber = null,
    string? EmergencyContact = null,
    string? ServiceClass = null,
    DefaultIdType? RateScheduleId = null,
    string? RateSchedule = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

