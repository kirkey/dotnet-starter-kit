using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;

public sealed record RejectValuationCommand(Guid Id, string Reason) : IRequest<RejectValuationResponse>;
