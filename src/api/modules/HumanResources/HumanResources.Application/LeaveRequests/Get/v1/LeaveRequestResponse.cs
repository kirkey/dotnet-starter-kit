namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;

/// <summary>
/// Response object for LeaveRequest entity details.
/// </summary>
public sealed record LeaveRequestResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public DefaultIdType LeaveTypeId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal NumberOfDays { get; init; }
    public string Reason { get; init; } = default!;
    public string Status { get; init; } = default!;
    public DefaultIdType? ApproverManagerId { get; init; }
    public DateTime? SubmittedDate { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public string? ApproverComment { get; init; }
    public bool IsActive { get; init; }
}
