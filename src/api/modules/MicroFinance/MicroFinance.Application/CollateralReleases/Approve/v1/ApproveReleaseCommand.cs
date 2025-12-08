using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;

public sealed record ApproveReleaseCommand(DefaultIdType Id, DefaultIdType ApprovedById) : IRequest<ApproveReleaseResponse>;
