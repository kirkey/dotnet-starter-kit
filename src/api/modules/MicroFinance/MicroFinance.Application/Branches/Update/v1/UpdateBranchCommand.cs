using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Update.v1;

public sealed record UpdateBranchCommand(
    Guid Id,
    string? Name = null,
    string? Address = null,
    string? City = null,
    string? State = null,
    string? Country = null,
    string? PostalCode = null,
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
