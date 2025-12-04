using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;

public sealed record ApproveReleaseCommand(Guid Id, Guid ApprovedById) : IRequest<ApproveReleaseResponse>;
