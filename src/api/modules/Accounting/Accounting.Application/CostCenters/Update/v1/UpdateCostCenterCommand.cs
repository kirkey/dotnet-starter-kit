namespace Accounting.Application.CostCenters.Update.v1;

public sealed record UpdateCostCenterCommand(
    DefaultIdType Id,
    string? Name = null,
    DefaultIdType? ManagerId = null,
    string? ManagerName = null,
    string? Location = null,
    DateTime? EndDate = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
