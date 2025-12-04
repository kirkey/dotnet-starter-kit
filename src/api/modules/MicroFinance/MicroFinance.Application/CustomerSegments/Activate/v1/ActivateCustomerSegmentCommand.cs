using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Activate.v1;

public sealed record ActivateCustomerSegmentCommand(Guid Id) : IRequest<ActivateCustomerSegmentResponse>;
