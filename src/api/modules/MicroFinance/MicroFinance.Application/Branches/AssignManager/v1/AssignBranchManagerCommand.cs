using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.AssignManager.v1;

public sealed record AssignBranchManagerCommand(
    Guid Id,
    string ManagerName,
    string? ManagerPhone = null,
    string? ManagerEmail = null) : IRequest<AssignBranchManagerResponse>;
