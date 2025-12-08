using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;

public sealed record GetCustomerSegmentRequest(DefaultIdType Id) : IRequest<CustomerSegmentResponse>;
