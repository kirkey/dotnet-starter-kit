using FSH.Starter.WebApi.HumanResources.Application.Enrollments.Specifications;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Get.v1;

/// <summary>
/// Handler for retrieving an enrollment by ID.
/// </summary>
public sealed class GetEnrollmentHandler(
    [FromKeyedServices("hr:enrollments")] IReadRepository<BenefitEnrollment> repository)
    : IRequestHandler<GetEnrollmentRequest, EnrollmentResponse>
{
    public async Task<EnrollmentResponse> Handle(
        GetEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        var enrollment = await repository
            .FirstOrDefaultAsync(new EnrollmentByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (enrollment is null)
            throw new NotFoundException($"Enrollment not found: {request.Id}");

        return MapToResponse(enrollment);
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

