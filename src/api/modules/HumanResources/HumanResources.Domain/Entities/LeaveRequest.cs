using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a leave request submitted by an employee.
/// Tracks request status, approvals, and attachment documents.
/// </summary>
public class LeaveRequest : AuditableEntity, IAggregateRoot
{
    private LeaveRequest() { }

    private LeaveRequest(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        string reason = "")
    {
        Id = id;
        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
        Status = "Draft";
        IsActive = true;
        NumberOfDays = CalculateBusinessDays(startDate, endDate);

        QueueDomainEvent(new LeaveRequestCreated { LeaveRequest = this });
    }

    /// <summary>
    /// The employee requesting leave.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The type of leave requested.
    /// </summary>
    public DefaultIdType LeaveTypeId { get; private set; }
    public LeaveType LeaveType { get; private set; } = default!;

    /// <summary>
    /// Start date of leave.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// End date of leave.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Number of leave days requested.
    /// </summary>
    public decimal NumberOfDays { get; private set; }

    /// <summary>
    /// Reason for leave.
    /// </summary>
    public string Reason { get; private set; }

    /// <summary>
    /// Status: Draft, Submitted, Approved, Rejected, Cancelled.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Manager assigned to approve this request.
    /// </summary>
    public DefaultIdType? ApproverManagerId { get; private set; }

    /// <summary>
    /// Date the request was submitted.
    /// </summary>
    public DateTime? SubmittedDate { get; private set; }

    /// <summary>
    /// Date the request was approved/rejected.
    /// </summary>
    public DateTime? ReviewedDate { get; private set; }

    /// <summary>
    /// Manager's comments.
    /// </summary>
    public string? ApproverComment { get; private set; }

    /// <summary>
    /// Whether this request is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Attachment file path (if any).
    /// </summary>
    public string? AttachmentPath { get; private set; }

    /// <summary>
    /// Calculates business days between two dates.
    /// </summary>
    private static decimal CalculateBusinessDays(DateTime startDate, DateTime endDate)
    {
        var days = 0m;
        var current = startDate;

        while (current <= endDate)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                days++;

            current = current.AddDays(1);
        }

        return days;
    }

    /// <summary>
    /// Creates a new leave request.
    /// </summary>
    public static LeaveRequest Create(
        DefaultIdType employeeId,
        DefaultIdType leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        string reason = "")
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before end date.", nameof(endDate));

        if (startDate < DateTime.Today)
            throw new ArgumentException("Start date cannot be in the past.", nameof(startDate));

        var request = new LeaveRequest(
            DefaultIdType.NewGuid(),
            employeeId,
            leaveTypeId,
            startDate,
            endDate,
            reason);

        return request;
    }

    /// <summary>
    /// Submits the leave request for approval.
    /// </summary>
    public LeaveRequest Submit(DefaultIdType approverId)
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Only draft requests can be submitted.");

        Status = "Submitted";
        SubmittedDate = DateTime.UtcNow;
        ApproverManagerId = approverId;

        QueueDomainEvent(new LeaveRequestSubmitted { LeaveRequest = this });
        return this;
    }

    /// <summary>
    /// Approves the leave request.
    /// </summary>
    public LeaveRequest Approve(string? comment = null)
    {
        if (Status != "Submitted")
            throw new InvalidOperationException("Only submitted requests can be approved.");

        Status = "Approved";
        ReviewedDate = DateTime.UtcNow;
        ApproverComment = comment;

        QueueDomainEvent(new LeaveRequestApproved { LeaveRequest = this });
        return this;
    }

    /// <summary>
    /// Rejects the leave request.
    /// </summary>
    public LeaveRequest Reject(string reason)
    {
        if (Status != "Submitted")
            throw new InvalidOperationException("Only submitted requests can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required.", nameof(reason));

        Status = "Rejected";
        ReviewedDate = DateTime.UtcNow;
        ApproverComment = reason;

        QueueDomainEvent(new LeaveRequestRejected { LeaveRequest = this });
        return this;
    }

    /// <summary>
    /// Cancels the leave request.
    /// </summary>
    public LeaveRequest Cancel(string reason = "")
    {
        if (Status is "Approved" or "Cancelled")
            throw new InvalidOperationException($"Cannot cancel {Status} requests.");

        Status = "Cancelled";
        ApproverComment = reason;

        QueueDomainEvent(new LeaveRequestCancelled { LeaveRequest = this });
        return this;
    }

    /// <summary>
    /// Attaches a supporting document.
    /// </summary>
    public LeaveRequest AttachDocument(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path is required.", nameof(filePath));

        AttachmentPath = filePath;
        return this;
    }
}

/// <summary>
/// Leave request status constants.
/// </summary>
public static class LeaveRequestStatus
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Cancelled = "Cancelled";
}

