using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Deactivate.v1;

public sealed class DeactivateCustomerSegmentHandler(
    [FromKeyedServices("microfinance:customersegments")] IRepository<CustomerSegment> repository,
    ILogger<DeactivateCustomerSegmentHandler> logger)
    : IRequestHandler<DeactivateCustomerSegmentCommand, DeactivateCustomerSegmentResponse>
{
    public async Task<DeactivateCustomerSegmentResponse> Handle(
        DeactivateCustomerSegmentCommand request,
        CancellationToken cancellationToken)
    {
        var segment = await repository.FirstOrDefaultAsync(
            new CustomerSegmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Customer segment {request.Id} not found");

        segment.Deactivate();
        await repository.UpdateAsync(segment, cancellationToken);

        logger.LogInformation("Customer segment deactivated: {SegmentId}", segment.Id);

        return new DeactivateCustomerSegmentResponse(segment.Id, segment.Status);
    }
}
