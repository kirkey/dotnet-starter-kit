using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Update.v1;

public sealed record UpdateBranchCommand(
    DefaultIdType Id,
    string? Name = null,
    string? Address = null,
    string? Phone = null,
    string? Email = null,
    string? ManagerName = null,
    string? ManagerPhone = null,
    string? ManagerEmail = null,
    decimal? Latitude = null,
    decimal? Longitude = null,
    string? OperatingHours = null,
    string? Timezone = null,
    decimal? CashHoldingLimit = null,
    string? Notes = null) : IRequest<UpdateBranchResponse>;
