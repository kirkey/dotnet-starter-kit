using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Close.v1;

public sealed record CloseBranchCommand(DefaultIdType Id, DateOnly? ClosingDate = null) : IRequest<CloseBranchResponse>;
