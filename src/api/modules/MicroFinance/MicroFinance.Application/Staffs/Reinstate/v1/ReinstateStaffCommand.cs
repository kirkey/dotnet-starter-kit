// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Reinstate/v1/ReinstateStaffCommand.cs

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Reinstate.v1;

/// <summary>
/// Command for reinstating a suspended staff member.
/// </summary>
public sealed record ReinstateStaffCommand(DefaultIdType Id) : IRequest<ReinstateStaffResponse>;

