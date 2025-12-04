using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a collateral release request when loan is paid off.
/// </summary>
public sealed class CollateralRelease : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int ReferenceMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int MethodMaxLength = 64;
    public const int NotesMaxLength = 512;
    public const int DocumentMaxLength = 512;
    
    // Release Status
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusReleased = "Released";
    public const string StatusCancelled = "Cancelled";
    
    // Release Methods
    public const string MethodInPerson = "InPerson";
    public const string MethodCourier = "Courier";
    public const string MethodElectronic = "Electronic";

    public Guid CollateralId { get; private set; }
    public Guid LoanId { get; private set; }
    public string ReleaseReference { get; private set; } = default!;
    public string Status { get; private set; } = StatusPending;
    public DateOnly RequestDate { get; private set; }
    public Guid RequestedById { get; private set; }
    public string? ReleaseMethod { get; private set; }
    public string? RecipientName { get; private set; }
    public string? RecipientIdNumber { get; private set; }
    public string? RecipientContact { get; private set; }
    public DateOnly? ApprovedDate { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public DateOnly? ReleasedDate { get; private set; }
    public Guid? ReleasedById { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? Notes { get; private set; }
    public string? ReleaseDocumentPath { get; private set; }
    public string? RecipientSignaturePath { get; private set; }
    public bool DocumentsReturned { get; private set; }
    public bool RegistrationCleared { get; private set; }

    private CollateralRelease() { }

    public static CollateralRelease Create(
        Guid collateralId,
        Guid loanId,
        string releaseReference,
        Guid requestedById,
        string? releaseMethod = null)
    {
        var release = new CollateralRelease
        {
            CollateralId = collateralId,
            LoanId = loanId,
            ReleaseReference = releaseReference,
            RequestedById = requestedById,
            ReleaseMethod = releaseMethod,
            RequestDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusPending
        };

        release.QueueDomainEvent(new CollateralReleaseRequested(release));
        return release;
    }

    public CollateralRelease Approve(Guid approvedById)
    {
        Status = StatusApproved;
        ApprovedById = approvedById;
        ApprovedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new CollateralReleaseApproved(Id, CollateralId, LoanId));
        return this;
    }

    public CollateralRelease Reject(string reason, Guid rejectedById)
    {
        Status = StatusRejected;
        RejectionReason = reason;
        ApprovedById = rejectedById;
        ApprovedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new CollateralReleaseRejected(Id, CollateralId, reason));
        return this;
    }

    public CollateralRelease Release(
        Guid releasedById,
        string recipientName,
        string? recipientIdNumber = null,
        string? recipientSignaturePath = null)
    {
        Status = StatusReleased;
        ReleasedById = releasedById;
        ReleasedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        RecipientName = recipientName;
        RecipientIdNumber = recipientIdNumber;
        RecipientSignaturePath = recipientSignaturePath;
        DocumentsReturned = true;
        QueueDomainEvent(new CollateralReleased(Id, CollateralId, LoanId));
        return this;
    }

    public CollateralRelease Cancel(string reason)
    {
        Status = StatusCancelled;
        Notes = reason;
        return this;
    }

    public CollateralRelease MarkRegistrationCleared()
    {
        RegistrationCleared = true;
        return this;
    }

    public CollateralRelease Update(
        string? recipientContact = null,
        string? notes = null,
        string? releaseDocumentPath = null)
    {
        if (recipientContact is not null) RecipientContact = recipientContact;
        if (notes is not null) Notes = notes;
        if (releaseDocumentPath is not null) ReleaseDocumentPath = releaseDocumentPath;

        QueueDomainEvent(new CollateralReleaseUpdated(this));
        return this;
    }
}
