using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Create.v1;

public sealed class CreateCustomerSegmentHandler(
    [FromKeyedServices("microfinance:customersegments")] IRepository<CustomerSegment> repository,
    ILogger<CreateCustomerSegmentHandler> logger)
    : IRequestHandler<CreateCustomerSegmentCommand, CreateCustomerSegmentResponse>
{
    public async Task<CreateCustomerSegmentResponse> Handle(
        CreateCustomerSegmentCommand request,
        CancellationToken cancellationToken)
    {
        var segment = CustomerSegment.Create(
            request.Name,
            request.Code,
            request.SegmentType,
            request.Description,
            request.Priority);

        await repository.AddAsync(segment, cancellationToken);

        logger.LogInformation("Customer segment created: {SegmentId}", segment.Id);

        return new CreateCustomerSegmentResponse(segment.Id);
    }
}
