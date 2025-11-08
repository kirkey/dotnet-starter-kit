namespace Accounting.Application.FixedAssets.UpdateMaintenance.v1;

public sealed record UpdateMaintenanceCommand(
    DefaultIdType Id,
    DateTime? LastMaintenanceDate,
    DateTime? NextMaintenanceDate
) : IRequest<DefaultIdType>;

