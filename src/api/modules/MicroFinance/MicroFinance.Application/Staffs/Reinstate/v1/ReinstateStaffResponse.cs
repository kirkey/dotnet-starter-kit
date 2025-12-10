// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Reinstate/v1/ReinstateStaffResponse.cs
namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Reinstate.v1;

/// <summary>
/// Response after reinstating a staff member.
/// </summary>
public sealed record ReinstateStaffResponse(DefaultIdType Id, string Status);

