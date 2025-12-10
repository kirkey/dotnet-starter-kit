// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Update/v1/UpdateStaffCommand.cs

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Update.v1;

/// <summary>
/// Command for updating a staff member.
/// </summary>
public sealed record UpdateStaffCommand(
    DefaultIdType Id,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? Phone,
    string? AlternatePhone,
    DateOnly? DateOfBirth,
    string? Gender,
    string? NationalId,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? PhotoUrl,
    string? Notes) : IRequest<UpdateStaffResponse>;

