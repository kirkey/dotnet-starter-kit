using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Create.v1;

public sealed record CreateCustomerSegmentCommand(
    string Name,
    string Code,
    string SegmentType,
    string? Description = null,
    int Priority = 0) : IRequest<CreateCustomerSegmentResponse>;
