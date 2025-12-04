using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Close.v1;

public sealed record CloseBranchCommand(Guid Id, DateOnly? ClosingDate = null) : IRequest<CloseBranchResponse>;
