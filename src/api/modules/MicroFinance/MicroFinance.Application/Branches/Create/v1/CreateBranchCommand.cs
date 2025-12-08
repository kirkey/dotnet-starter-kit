using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Create.v1;

/// <summary>
/// Command to create a new branch.
/// </summary>
public sealed record CreateBranchCommand(
    [property: DefaultValue("BR-001")] string Code,
    [property: DefaultValue("Main Branch")] string Name,
    [property: DefaultValue("Branch")] string BranchType,
    [property: DefaultValue(null)] DefaultIdType? ParentBranchId = null,
    [property: DefaultValue("123 Main St")] string? Address = null,
    [property: DefaultValue("Manila")] string? City = null,
    [property: DefaultValue("Metro Manila")] string? State = null,
    [property: DefaultValue("Philippines")] string? Country = null,
    [property: DefaultValue("+63-2-1234567")] string? Phone = null,
    [property: DefaultValue("branch@mfi.com")] string? Email = null,
    [property: DefaultValue(null)] DateOnly? OpeningDate = null)
    : IRequest<CreateBranchResponse>;
