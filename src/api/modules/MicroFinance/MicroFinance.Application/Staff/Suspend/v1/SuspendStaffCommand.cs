// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Suspend/v1/SuspendStaffCommand.cs
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Suspend.v1;

/// <summary>
/// Command for suspending a staff member.
/// </summary>
public sealed record SuspendStaffCommand(Guid Id, string? Reason = null) : IRequest<SuspendStaffResponse>;

