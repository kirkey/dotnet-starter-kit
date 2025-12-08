using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Activate.v1;

public sealed record ActivateBranchCommand(DefaultIdType Id) : IRequest<ActivateBranchResponse>;
