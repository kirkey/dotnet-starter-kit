using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Deactivate.v1;

public sealed record DeactivateBranchCommand(Guid Id) : IRequest<DeactivateBranchResponse>;
