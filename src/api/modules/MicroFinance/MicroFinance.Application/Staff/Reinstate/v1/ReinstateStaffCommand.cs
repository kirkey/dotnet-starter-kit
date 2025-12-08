// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Reinstate/v1/ReinstateStaffCommand.cs
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Reinstate.v1;

/// <summary>
/// Command for reinstating a suspended staff member.
/// </summary>
public sealed record ReinstateStaffCommand(Guid Id) : IRequest<ReinstateStaffResponse>;

