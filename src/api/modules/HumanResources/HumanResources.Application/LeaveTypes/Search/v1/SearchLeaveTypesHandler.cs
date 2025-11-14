using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Search.v1;

public sealed class SearchLeaveTypesHandler(
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> repository)
    : IRequestHandler<SearchLeaveTypesRequest, PagedList<LeaveTypeResponse>>
{
    public async Task<PagedList<LeaveTypeResponse>> Handle(
        SearchLeaveTypesRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchLeaveTypesSpec(request);
        var leaveTypes = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = leaveTypes.Select(lt => new LeaveTypeResponse
        {
            Id = lt.Id,
            LeaveName = lt.LeaveName,
            AnnualAllowance = lt.AnnualAllowance,
            AccrualFrequency = lt.AccrualFrequency,
            IsPaid = lt.IsPaid,
            MaxCarryoverDays = lt.MaxCarryoverDays,
            CarryoverExpiryMonths = lt.CarryoverExpiryMonths,
            RequiresApproval = lt.RequiresApproval,
            MinimumNoticeDay = lt.MinimumNoticeDay,
            IsActive = lt.IsActive,
            Description = lt.Description
        }).ToList();

        return new PagedList<LeaveTypeResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

