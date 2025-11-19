namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Search.v1;

using Framework.Core.Paging;
using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching benefit allocations.
/// </summary>
public sealed class SearchBenefitAllocationsHandler(
    [FromKeyedServices("hr:benefitallocations")] IReadRepository<BenefitAllocation> repository)
    : IRequestHandler<SearchBenefitAllocationsRequest, PagedList<BenefitAllocationDto>>
{
    public async Task<PagedList<BenefitAllocationDto>> Handle(
        SearchBenefitAllocationsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBenefitAllocationsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(a => new BenefitAllocationDto(
            a.Id,
            a.EnrollmentId,
            a.Enrollment.Employee.Name,
            a.Enrollment.Benefit.BenefitName,
            a.AllocationDate,
            a.AllocatedAmount,
            a.AllocationType,
            a.Status)).ToList();

        return new PagedList<BenefitAllocationDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

