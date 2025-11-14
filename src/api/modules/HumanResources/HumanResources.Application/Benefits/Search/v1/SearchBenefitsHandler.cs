using FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;

/// <summary>
/// Handler for searching benefits.
/// </summary>
public sealed class SearchBenefitsHandler(
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> repository)
    : IRequestHandler<SearchBenefitsRequest, PagedList<BenefitResponse>>
{
    public async Task<PagedList<BenefitResponse>> Handle(
        SearchBenefitsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchBenefitsSpec(request);
        var benefits = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = benefits.Select(MapToResponse).ToList();

        return new PagedList<BenefitResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static BenefitResponse MapToResponse(Benefit benefit)
    {
        return new BenefitResponse
        {
            Id = benefit.Id,
            BenefitName = benefit.BenefitName,
            BenefitType = benefit.BenefitType,
            EmployeeContribution = benefit.EmployeeContribution,
            EmployerContribution = benefit.EmployerContribution,
            IsRequired = benefit.IsRequired,
            IsActive = benefit.IsActive,
            Description = benefit.Description,
            AnnualLimit = benefit.AnnualLimit,
            IsCarryoverAllowed = benefit.IsCarryoverAllowed,
            MinimumEligibleEmployees = benefit.MinimumEligibleEmployees,
            PayComponentId = benefit.PayComponentId
        };
    }
}

