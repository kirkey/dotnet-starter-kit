using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Get.v1;

/// <summary>
/// Specification for getting a customer case by ID.
/// </summary>
public sealed class CustomerCaseByIdSpec : Specification<CustomerCase, CustomerCaseResponse>
{
    public CustomerCaseByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);

        Query.Select(c => new CustomerCaseResponse(
            c.Id,
            c.CaseNumber,
            c.MemberId,
            c.Subject,
            c.Category,
            c.Priority,
            c.Status,
            c.Description,
            c.Channel,
            c.AssignedToId,
            c.OpenedAt,
            c.FirstResponseAt,
            c.ResolvedAt,
            c.ClosedAt,
            c.Resolution,
            c.EscalationLevel,
            c.SlaBreached,
            c.CustomerSatisfactionScore));
    }
}
