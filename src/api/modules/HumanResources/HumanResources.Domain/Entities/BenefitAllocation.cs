namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

using Events;

/// <summary>
/// Represents the allocation or usage of a benefit by an employee.
/// Tracks benefit usage such as leave days used, medical claims, etc.
/// </summary>
public class BenefitAllocation : AuditableEntity, IAggregateRoot
{
    private BenefitAllocation() { }

    private BenefitAllocation(
        DefaultIdType id,
        DefaultIdType enrollmentId,
        DateTime allocationDate,
        decimal allocatedAmount,
        string allocationType)
    {
        Id = id;
        EnrollmentId = enrollmentId;
        AllocationDate = allocationDate;
        AllocatedAmount = allocatedAmount;
        AllocationType = allocationType;
        Status = AllocationStatus.Pending;

        QueueDomainEvent(new BenefitAllocationCreated { Allocation = this });
    }

    /// <summary>
    /// The benefit enrollment this allocation is for.
    /// </summary>
    public DefaultIdType EnrollmentId { get; private set; }
    public BenefitEnrollment Enrollment { get; private set; } = default!;

    /// <summary>
    /// Date of the allocation/usage.
    /// </summary>
    public DateTime AllocationDate { get; private set; }

    /// <summary>
    /// Amount allocated or used (days, dollars, etc.).
    /// </summary>
    public decimal AllocatedAmount { get; private set; }

    /// <summary>
    /// Type of allocation (Usage, Claim, Reimbursement, etc.).
    /// </summary>
    public string AllocationType { get; private set; } = default!;

    /// <summary>
    /// Status of the allocation (Pending, Approved, Rejected, Paid).
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Reference number (claim number, leave request number, etc.).
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Approval date.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }

    /// <summary>
    /// Approved by (manager/HR).
    /// </summary>
    public DefaultIdType? ApprovedBy { get; private set; }

    /// <summary>
    /// Payment date (for reimbursements).
    /// </summary>
    public DateTime? PaymentDate { get; private set; }

    /// <summary>
    /// Notes or remarks about the allocation.
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    /// Creates a new benefit allocation.
    /// </summary>
    public static BenefitAllocation Create(
        DefaultIdType enrollmentId,
        DateTime allocationDate,
        decimal allocatedAmount,
        string allocationType)
    {
        if (allocatedAmount <= 0)
            throw new ArgumentException("Allocated amount must be greater than zero.", nameof(allocatedAmount));

        if (string.IsNullOrWhiteSpace(allocationType))
            throw new ArgumentException("Allocation type is required.", nameof(allocationType));

        var allocation = new BenefitAllocation(
            DefaultIdType.NewGuid(),
            enrollmentId,
            allocationDate,
            allocatedAmount,
            allocationType);

        return allocation;
    }

    /// <summary>
    /// Sets the reference number for tracking.
    /// </summary>
    public BenefitAllocation SetReferenceNumber(string referenceNumber)
    {
        ReferenceNumber = referenceNumber;
        return this;
    }

    /// <summary>
    /// Sets remarks or notes.
    /// </summary>
    public BenefitAllocation SetRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }

    /// <summary>
    /// Approves the allocation.
    /// </summary>
    public BenefitAllocation Approve(DefaultIdType approvedBy)
    {
        if (Status == AllocationStatus.Approved)
            throw new InvalidOperationException("Allocation is already approved.");

        Status = AllocationStatus.Approved;
        ApprovalDate = DateTime.UtcNow;
        ApprovedBy = approvedBy;

        QueueDomainEvent(new BenefitAllocationApproved { Allocation = this });
        return this;
    }

    /// <summary>
    /// Rejects the allocation.
    /// </summary>
    public BenefitAllocation Reject(DefaultIdType rejectedBy, string? reason = null)
    {
        if (Status == AllocationStatus.Rejected)
            throw new InvalidOperationException("Allocation is already rejected.");

        Status = AllocationStatus.Rejected;
        ApprovalDate = DateTime.UtcNow;
        ApprovedBy = rejectedBy;
        Remarks = reason ?? Remarks;

        QueueDomainEvent(new BenefitAllocationRejected { Allocation = this });
        return this;
    }

    /// <summary>
    /// Marks the allocation as paid (for reimbursements).
    /// </summary>
    public BenefitAllocation MarkAsPaid(DateTime paymentDate)
    {
        if (Status != AllocationStatus.Approved)
            throw new InvalidOperationException("Allocation must be approved before marking as paid.");

        Status = AllocationStatus.Paid;
        PaymentDate = paymentDate;

        QueueDomainEvent(new BenefitAllocationPaid { Allocation = this });
        return this;
    }

    /// <summary>
    /// Cancels the allocation.
    /// </summary>
    public BenefitAllocation Cancel()
    {
        if (Status == AllocationStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid allocation.");

        Status = AllocationStatus.Cancelled;

        QueueDomainEvent(new BenefitAllocationCancelled { Allocation = this });
        return this;
    }
}

/// <summary>
/// Allocation type constants.
/// </summary>
public static class AllocationType
{
    /// <summary>Benefit usage (e.g., leave days used).</summary>
    public const string Usage = "Usage";

    /// <summary>Medical/health claim.</summary>
    public const string Claim = "Claim";

    /// <summary>Reimbursement request.</summary>
    public const string Reimbursement = "Reimbursement";

    /// <summary>Deduction from benefit.</summary>
    public const string Deduction = "Deduction";

    /// <summary>Adjustment to benefit.</summary>
    public const string Adjustment = "Adjustment";
}

/// <summary>
/// Allocation status constants.
/// </summary>
public static class AllocationStatus
{
    /// <summary>Pending approval.</summary>
    public const string Pending = "Pending";

    /// <summary>Approved by manager/HR.</summary>
    public const string Approved = "Approved";

    /// <summary>Rejected.</summary>
    public const string Rejected = "Rejected";

    /// <summary>Payment processed (for reimbursements).</summary>
    public const string Paid = "Paid";

    /// <summary>Cancelled.</summary>
    public const string Cancelled = "Cancelled";
}

