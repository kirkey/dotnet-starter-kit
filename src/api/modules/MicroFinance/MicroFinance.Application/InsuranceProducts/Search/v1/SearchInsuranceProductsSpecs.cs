// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InsuranceProducts/Search/v1/SearchInsuranceProductsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Search.v1;

/// <summary>
/// Specification for searching insurance products.
/// </summary>
public sealed class SearchInsuranceProductsSpecs : Specification<InsuranceProduct, InsuranceProductResponse>
{
    public SearchInsuranceProductsSpecs(SearchInsuranceProductsCommand command)
    {
        Query.OrderBy(p => p.Name);

        if (!string.IsNullOrEmpty(command.Name))
        {
            Query.Where(p => p.Name.Contains(command.Name));
        }

        if (!string.IsNullOrEmpty(command.Code))
        {
            Query.Where(p => p.Code.Contains(command.Code));
        }

        if (!string.IsNullOrEmpty(command.InsuranceType))
        {
            Query.Where(p => p.InsuranceType == command.InsuranceType);
        }

        if (!string.IsNullOrEmpty(command.Provider))
        {
            Query.Where(p => p.Provider == command.Provider);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(p => p.Status == command.Status);
        }

        if (command.MandatoryWithLoan.HasValue)
        {
            Query.Where(p => p.MandatoryWithLoan == command.MandatoryWithLoan.Value);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(p => new InsuranceProductResponse(
            p.Id,
            p.Code,
            p.Name,
            p.InsuranceType,
            p.Provider,
            p.MinCoverage,
            p.MaxCoverage,
            p.PremiumCalculation,
            p.PremiumRate,
            p.MinAge,
            p.MaxAge,
            p.WaitingPeriodDays,
            p.PremiumUpfront,
            p.MandatoryWithLoan,
            p.Status,
            p.Description,
            p.CoveredEvents,
            p.Exclusions,
            p.TermsConditions,
            p.Notes));
    }
}

