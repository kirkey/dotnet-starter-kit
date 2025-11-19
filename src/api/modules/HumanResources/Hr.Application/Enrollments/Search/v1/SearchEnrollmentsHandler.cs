using FSH.Starter.WebApi.HumanResources.Application.Enrollments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Enrollments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Search.v1;

/// <summary>
/// Handler for searching enrollments.
/// </summary>
public sealed class SearchEnrollmentsHandler(
    [FromKeyedServices("hr:enrollments")] IReadRepository<BenefitEnrollment> repository)
    : IRequestHandler<SearchEnrollmentsRequest, PagedList<EnrollmentResponse>>
{
    public async Task<PagedList<EnrollmentResponse>> Handle(
        SearchEnrollmentsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEnrollmentsSpec(request);
        var enrollments = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = enrollments.Select(MapToResponse).ToList();

        return new PagedList<EnrollmentResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static EnrollmentResponse MapToResponse(BenefitEnrollment enrollment)
    {
        return new EnrollmentResponse
        {
            Id = enrollment.Id,
            EmployeeId = enrollment.EmployeeId,
            BenefitId = enrollment.BenefitId,
            EnrollmentDate = enrollment.EnrollmentDate,
            EffectiveDate = enrollment.EffectiveDate,
            CoverageLevel = enrollment.CoverageLevel,
            EmployeeContributionAmount = enrollment.EmployeeContributionAmount,
            EmployerContributionAmount = enrollment.EmployerContributionAmount,
            AnnualContribution = enrollment.AnnualContribution,
            EndDate = enrollment.EndDate,
            IsActive = enrollment.IsActive,
            CoveredDependentIds = enrollment.CoveredDependentIds
        };
    }
}

