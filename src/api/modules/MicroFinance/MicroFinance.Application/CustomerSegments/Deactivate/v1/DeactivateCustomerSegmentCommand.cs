using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Deactivate.v1;

public sealed record DeactivateCustomerSegmentCommand(Guid Id) : IRequest<DeactivateCustomerSegmentResponse>;
