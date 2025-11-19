namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;

/// <summary>
/// Response object for LeaveBalance entity details.
/// </summary>
public sealed record LeaveBalanceResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public DefaultIdType LeaveTypeId { get; init; }
    public int Year { get; init; }
    public decimal OpeningBalance { get; init; }
    public decimal AccruedDays { get; init; }
    public decimal CarriedOverDays { get; init; }
    public decimal AvailableDays { get; init; }
    public decimal TakenDays { get; init; }
    public decimal PendingDays { get; init; }
    public decimal RemainingDays { get; init; }
    public DateTime? CarryoverExpiryDate { get; init; }
}
