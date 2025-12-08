using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Approve.v1;

public sealed record ApproveValuationCommand(DefaultIdType Id, DefaultIdType ApprovedById) : IRequest<ApproveValuationResponse>;
