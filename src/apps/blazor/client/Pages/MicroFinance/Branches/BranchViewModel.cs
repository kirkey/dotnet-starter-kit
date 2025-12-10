namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Branches;

/// <summary>
/// ViewModel used by the Branches page for add/edit operations.
/// Extends UpdateBranchCommand to follow the pattern used in Product and Catalog pages.
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
    /// Branch status: "Pending", "Active", "Inactive", "Closed".
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Parent branch ID for hierarchical structure.
    /// </summary>
    public DefaultIdType? ParentBranchId { get; set; }

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
    /// Manager name.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// Manager phone number.
    /// </summary>
    public string? ManagerPhone { get; set; }

    /// <summary>
    /// Manager email address.
    /// </summary>
    public string? ManagerEmail { get; set; }

    /// <summary>
    /// Branch opening date.
    /// </summary>
    public DateOnly? OpeningDate { get; set; }

    /// <summary>
    /// Branch closing date.
    /// </summary>
    public DateOnly? ClosingDate { get; set; }

    /// <summary>
    /// Latitude coordinate.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Longitude coordinate.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    /// Operating hours description.
    /// </summary>
    public string? OperatingHours { get; set; }

    /// <summary>
    /// Branch timezone.
    /// </summary>
    public string? Timezone { get; set; }

    /// <summary>
    /// Cash holding limit for the branch.
    /// </summary>
    public decimal? CashHoldingLimit { get; set; }

    /// <summary>
    /// Additional notes about the branch.
    /// </summary>
    public string? Notes { get; set; }
}
