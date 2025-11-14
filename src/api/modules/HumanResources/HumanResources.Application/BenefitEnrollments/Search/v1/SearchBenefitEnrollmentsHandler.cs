namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Search.v1;

using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching benefit enrollments.
/// </summary>
public sealed class SearchBenefitEnrollmentsHandler(
    [FromKeyedServices("hr:benefitenrollments")] IReadRepository<BenefitEnrollment> repository)
    : IRequestHandler<SearchBenefitEnrollmentsRequest, PagedList<BenefitEnrollmentDto>>
{
    public async Task<PagedList<BenefitEnrollmentDto>> Handle(
        SearchBenefitEnrollmentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBenefitEnrollmentsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(e => new BenefitEnrollmentDto(
            e.Id,
            e.EmployeeId,
            e.Employee.Name,
            e.BenefitId,
            e.Benefit.BenefitName,
            e.EffectiveDate,
            e.CoverageLevel,
            e.AnnualContribution,
            e.IsActive)).ToList();

        return new PagedList<BenefitEnrollmentDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

