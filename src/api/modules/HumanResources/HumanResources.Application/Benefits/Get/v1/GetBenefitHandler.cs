namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting benefit details.
/// </summary>
public sealed class GetBenefitHandler(
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> repository)
    : IRequestHandler<GetBenefitRequest, BenefitResponse>
{
    public async Task<BenefitResponse> Handle(
        GetBenefitRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BenefitByIdSpec(request.Id);
        var benefit = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (benefit is null)
            throw new BenefitNotFoundException(request.Id);

        return new BenefitResponse(
            benefit.Id,
            benefit.BenefitName,
            benefit.BenefitType,
            benefit.EmployeeContribution,
            benefit.EmployerContribution,
            benefit.IsMandatory,
            benefit.IsActive,
            benefit.EffectiveStartDate,
            benefit.EffectiveEndDate,
            benefit.CoverageType,
            benefit.ProviderName,
            benefit.CoverageAmount,
            benefit.WaitingPeriodDays,
            benefit.Description,
            benefit.ImageUrl);
    }
}

