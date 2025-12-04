using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Activate.v1;

public sealed record ActivateBranchCommand(Guid Id) : IRequest<ActivateBranchResponse>;
