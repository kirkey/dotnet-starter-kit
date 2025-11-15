namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Get.v1;

using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting benefit enrollment details.
/// </summary>
public sealed class GetBenefitEnrollmentHandler(
    [FromKeyedServices("hr:benefitenrollments")] IReadRepository<BenefitEnrollment> repository)
    : IRequestHandler<GetBenefitEnrollmentRequest, BenefitEnrollmentResponse>
{
    public async Task<BenefitEnrollmentResponse> Handle(
        GetBenefitEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BenefitEnrollmentByIdSpec(request.Id);
        var enrollment = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (enrollment is null)
            throw new BenefitEnrollmentNotFoundException(request.Id);

        return new BenefitEnrollmentResponse(
            enrollment.Id,
            enrollment.EmployeeId,
            enrollment.Employee.Name,
            enrollment.BenefitId,
            enrollment.Benefit.BenefitName,
            enrollment.EnrollmentDate,
            enrollment.EffectiveDate,
            enrollment.EndDate,
            enrollment.CoverageLevel,
            enrollment.EmployeeContributionAmount,
            enrollment.EmployerContributionAmount,
            enrollment.AnnualContribution,
            enrollment.IsActive,
            enrollment.CoveredDependentIds);
    }
}

