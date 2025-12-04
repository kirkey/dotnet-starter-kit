using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;

public sealed class GetCustomerSegmentHandler(
    [FromKeyedServices("microfinance:customersegments")] IReadRepository<CustomerSegment> repository)
    : IRequestHandler<GetCustomerSegmentRequest, CustomerSegmentResponse>
{
    public async Task<CustomerSegmentResponse> Handle(
        GetCustomerSegmentRequest request,
        CancellationToken cancellationToken)
    {
        var segment = await repository.FirstOrDefaultAsync(
            new CustomerSegmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Customer segment {request.Id} not found");

        return new CustomerSegmentResponse(
            segment.Id,
            segment.Name,
            segment.Code,
            segment.Description,
            segment.Status,
            segment.SegmentType,
            segment.SegmentCriteria,
            segment.Priority,
            segment.MemberCount,
            segment.MinIncomeLevel,
            segment.MaxIncomeLevel,
            segment.RiskLevel,
            segment.DefaultInterestModifier,
            segment.DefaultFeeModifier);
    }
}
