namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;

using Framework.Core.Paging;
using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching benefits.
/// </summary>
public sealed class SearchBenefitsHandler(
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> repository)
    : IRequestHandler<SearchBenefitsRequest, PagedList<BenefitDto>>
{
    public async Task<PagedList<BenefitDto>> Handle(
        SearchBenefitsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBenefitsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(b => new BenefitDto(
            b.Id,
            b.BenefitName,
            b.BenefitType,
            b.EmployeeContribution,
            b.EmployerContribution,
            b.IsMandatory,
            b.IsActive)).ToList();

        return new PagedList<BenefitDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

