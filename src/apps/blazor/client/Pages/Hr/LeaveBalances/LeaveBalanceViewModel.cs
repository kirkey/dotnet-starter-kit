namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveBalances;

/// <summary>
/// ViewModel for LeaveBalance CRUD operations.
/// Represents employee leave balances by leave type and year.
/// </summary>
public class LeaveBalanceViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public DefaultIdType LeaveTypeId { get; set; }
    public int Year { get; set; } = DateTime.Today.Year;
    public decimal OpeningBalance { get; set; }
    public decimal AccruedDays { get; set; }
    public decimal CarriedOverDays { get; set; }
    public decimal AvailableDays { get; set; }
    public decimal TakenDays { get; set; }
    public decimal PendingDays { get; set; }
    public decimal RemainingDays { get; set; }
    public DateTime? CarryoverExpiryDate { get; set; }
}
