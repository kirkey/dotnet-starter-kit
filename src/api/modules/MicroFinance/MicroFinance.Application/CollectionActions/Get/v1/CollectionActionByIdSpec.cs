using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

/// <summary>
/// Specification for getting a collection action by ID.
/// </summary>
public sealed class CollectionActionByIdSpec : Specification<CollectionAction, CollectionActionResponse>
{
    public CollectionActionByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);

        Query.Select(c => new CollectionActionResponse(
            c.Id,
            c.CollectionCaseId,
            c.LoanId,
            c.ActionType,
            c.ActionDateTime,
            c.PerformedById,
            c.ContactMethod,
            c.PhoneNumberCalled,
            c.ContactPerson,
            c.Outcome,
            c.Description,
            c.PromisedAmount,
            c.PromisedDate,
            c.NextFollowUpDate));
    }
}
