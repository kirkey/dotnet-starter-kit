namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

/// <summary>
/// Response object for LeaveType entity details.
/// </summary>
public sealed record LeaveTypeResponse
{
    public DefaultIdType Id { get; init; }
    public string LeaveName { get; init; } = default!;
    public decimal AnnualAllowance { get; init; }
    public string AccrualFrequency { get; init; } = default!;
    public bool IsPaid { get; init; }
    public decimal MaxCarryoverDays { get; init; }
    public int? CarryoverExpiryMonths { get; init; }
    public bool RequiresApproval { get; init; }
    public int? MinimumNoticeDay { get; init; }
    public bool IsActive { get; init; }
    public string? Description { get; init; }
}
