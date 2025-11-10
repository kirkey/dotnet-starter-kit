namespace Accounting.Application.Members.Update.v1;

/// <summary>
/// Command to update a member account.
/// </summary>
public sealed record UpdateMemberCommand(
    DefaultIdType Id,
    string? MemberName = null,
    string? ServiceAddress = null,
    string? MailingAddress = null,
    string? ContactInfo = null,
    string? AccountStatus = null,
    DefaultIdType? MeterId = null,
    string? Email = null,
    string? PhoneNumber = null,
    string? EmergencyContact = null,
    string? ServiceClass = null,
    string? RateSchedule = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

