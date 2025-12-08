using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Activate.v1;

public sealed record ActivateCustomerSegmentCommand(DefaultIdType Id) : IRequest<ActivateCustomerSegmentResponse>;
