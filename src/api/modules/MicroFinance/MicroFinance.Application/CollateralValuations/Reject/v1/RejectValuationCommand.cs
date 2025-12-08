using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;

public sealed record RejectValuationCommand(DefaultIdType Id, string Reason) : IRequest<RejectValuationResponse>;
