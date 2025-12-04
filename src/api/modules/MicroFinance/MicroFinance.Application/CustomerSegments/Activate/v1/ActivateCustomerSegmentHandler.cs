using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Activate.v1;

public sealed class ActivateCustomerSegmentHandler(
    [FromKeyedServices("microfinance:customersegments")] IRepository<CustomerSegment> repository,
    ILogger<ActivateCustomerSegmentHandler> logger)
    : IRequestHandler<ActivateCustomerSegmentCommand, ActivateCustomerSegmentResponse>
{
    public async Task<ActivateCustomerSegmentResponse> Handle(
        ActivateCustomerSegmentCommand request,
        CancellationToken cancellationToken)
    {
        var segment = await repository.FirstOrDefaultAsync(
            new CustomerSegmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Customer segment {request.Id} not found");

        segment.Activate();
        await repository.UpdateAsync(segment, cancellationToken);

        logger.LogInformation("Customer segment activated: {SegmentId}", segment.Id);

        return new ActivateCustomerSegmentResponse(segment.Id, segment.Status);
    }
}
