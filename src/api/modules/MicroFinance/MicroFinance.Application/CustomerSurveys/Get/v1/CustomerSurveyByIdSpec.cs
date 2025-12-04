using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;

/// <summary>
/// Specification to get a customer survey by ID.
/// </summary>
public sealed class CustomerSurveyByIdSpec : Specification<CustomerSurvey>, ISingleResultSpecification<CustomerSurvey>
{
    public CustomerSurveyByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
