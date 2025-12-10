namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Branches;

/// <summary>
/// ViewModel used by the Branches page for add/edit operations.
/// Mirrors the shape of the API's CreateBranchCommand and UpdateBranchCommand.
/// </summary>
public class BranchViewModel
{
    /// <summary>
    /// Primary identifier of the branch.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique branch code. Example: "BR-001", "HQ-001".
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Branch name. Required.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Type of branch: "Headquarters", "Regional", "Branch", "SubBranch", "Agency".
    /// </summary>
    public string? BranchType { get; set; }

    /// <summary>
    /// Parent branch ID for hierarchical structure.
    /// </summary>
    public Guid? ParentBranchId { get; set; }

    /// <summary>
    /// Physical address of the branch.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Branch phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Branch email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Branch opening date.
    /// </summary>
    public DateOnly? OpeningDate { get; set; }

    /// <summary>
    /// DateTime wrapper for OpeningDate to work with MudDatePicker.
    /// </summary>
    public DateTime? OpeningDateDate
    {
        get => OpeningDate?.ToDateTime(TimeOnly.MinValue);
        set => OpeningDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Cash holding limit for the branch.
    /// </summary>
    public decimal? CashHoldingLimit { get; set; }

    /// <summary>
    /// Additional notes about the branch.
    /// </summary>
    public string? Notes { get; set; }
}
