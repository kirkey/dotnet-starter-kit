using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Search.v1;

public sealed class SearchLeaveRequestsHandler(
    [FromKeyedServices("hr:leaverequests")] IReadRepository<LeaveRequest> repository)
    : IRequestHandler<SearchLeaveRequestsRequest, PagedList<LeaveRequestResponse>>
{
    public async Task<PagedList<LeaveRequestResponse>> Handle(
        SearchLeaveRequestsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchLeaveRequestsSpec(request);
        var requests = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = requests.Select(lr => new LeaveRequestResponse
        {
            Id = lr.Id,
            EmployeeId = lr.EmployeeId,
            LeaveTypeId = lr.LeaveTypeId,
            StartDate = lr.StartDate,
            EndDate = lr.EndDate,
            NumberOfDays = lr.NumberOfDays,
            Reason = lr.Reason,
            Status = lr.Status,
            ApproverManagerId = lr.ApproverManagerId,
            SubmittedDate = lr.SubmittedDate,
            ReviewedDate = lr.ReviewedDate,
            ApproverComment = lr.ApproverComment,
            IsActive = lr.IsActive
        }).ToList();

        return new PagedList<LeaveRequestResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

