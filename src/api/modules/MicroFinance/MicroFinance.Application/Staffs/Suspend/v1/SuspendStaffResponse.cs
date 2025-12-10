// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Suspend/v1/SuspendStaffResponse.cs
namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Suspend.v1;

/// <summary>
/// Response after suspending a staff member.
/// </summary>
public sealed record SuspendStaffResponse(DefaultIdType Id, string Status);

