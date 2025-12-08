namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanSchedules;

/// <summary>
/// ViewModel for loan schedule entities (read-only view).
/// Loan schedules are generated automatically from loan disbursement.
/// </summary>
public class LoanScheduleViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The loan this schedule belongs to.
    /// </summary>
    public Guid LoanId { get; set; }
    
    /// <summary>
    /// Installment number in the schedule.
    /// </summary>
    public int InstallmentNumber { get; set; }
    
    /// <summary>
    /// Due date for this installment.
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Principal amount due.
    /// </summary>
    public decimal PrincipalAmount { get; set; }
    
    /// <summary>
    /// Interest amount due.
    /// </summary>
    public decimal InterestAmount { get; set; }
    
    /// <summary>
    /// Total amount due for this installment.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
