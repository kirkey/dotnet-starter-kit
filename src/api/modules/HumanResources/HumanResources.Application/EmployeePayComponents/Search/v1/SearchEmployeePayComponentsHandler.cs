namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;
using Specifications;

/// <summary>
/// Handler for searching employee pay components with filters and pagination.
/// </summary>
public sealed class SearchEmployeePayComponentsHandler(
    [FromKeyedServices("hr:employeepaycomponents")] IReadRepository<EmployeePayComponent> repository)
    : IRequestHandler<SearchEmployeePayComponentsRequest, PagedList<EmployeePayComponentResponse>>
{
    public async Task<PagedList<EmployeePayComponentResponse>> Handle(
        SearchEmployeePayComponentsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEmployeePayComponentsSpec(request);
        var components = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = components.Select(epc => new EmployeePayComponentResponse(
            epc.Id,
            epc.EmployeeId,
            epc.PayComponentId,
            epc.AssignmentType,
            epc.CustomRate,
            epc.FixedAmount,
            epc.CustomFormula,
            epc.EffectiveStartDate,
            epc.EffectiveEndDate,
            epc.IsActive,
            epc.IsRecurring,
            epc.IsOneTime,
            epc.OneTimeDate,
            epc.InstallmentCount,
            null, // CurrentInstallment
            null, // TotalAmount
            null, // RemainingBalance
            null, // ReferenceNumber
            null, // ApprovedBy
            null, // ApprovedDate
            null  // Remarks
        )).ToList();

        return new PagedList<EmployeePayComponentResponse>(
            responses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

