namespace Accounting.Domain.Constants;

/// <summary>
/// Constants for bill status values.
/// </summary>
/// <remarks>
/// Bill status represents the lifecycle stage of a vendor bill.
/// Status progression typically follows: Draft → Submitted → Approved → Posted → Paid.
/// </remarks>
public static class BillStatus
{
    /// <summary>
    /// Bill is in draft state and can be edited freely.
    /// </summary>
    public const string Draft = "Draft";

    /// <summary>
    /// Bill has been submitted for approval.
    /// </summary>
    public const string Submitted = "Submitted";

    /// <summary>
    /// Bill has been approved and is ready for posting.
    /// </summary>
    public const string Approved = "Approved";

    /// <summary>
    /// Bill has been rejected during approval.
    /// </summary>
    public const string Rejected = "Rejected";

    /// <summary>
    /// Bill has been posted to the general ledger.
    /// </summary>
    public const string Posted = "Posted";

    /// <summary>
    /// Bill has been fully paid.
    /// </summary>
    public const string Paid = "Paid";

    /// <summary>
    /// Bill has been voided and is no longer valid.
    /// </summary>
    public const string Void = "Void";
}

/// <summary>
/// Constants for bill approval status values.
/// </summary>
public static class BillApprovalStatus
{
    /// <summary>
    /// Bill is pending approval.
    /// </summary>
    public const string Pending = "Pending";

    /// <summary>
    /// Bill has been approved.
    /// </summary>
    public const string Approved = "Approved";

    /// <summary>
    /// Bill has been rejected.
    /// </summary>
    public const string Rejected = "Rejected";
}

