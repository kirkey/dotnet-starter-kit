using FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

/// <summary>
/// Handler for retrieving a benefit by ID.
/// </summary>
public sealed class GetBenefitHandler(
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> repository)
    : IRequestHandler<GetBenefitRequest, BenefitResponse>
{
    public async Task<BenefitResponse> Handle(
        GetBenefitRequest request,
        CancellationToken cancellationToken)
    {
        var benefit = await repository
            .FirstOrDefaultAsync(new BenefitByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (benefit is null)
            throw new Exception($"Benefit not found: {request.Id}");

        return MapToResponse(benefit);
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

