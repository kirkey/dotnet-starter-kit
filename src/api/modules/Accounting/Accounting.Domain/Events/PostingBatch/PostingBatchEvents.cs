using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.PostingBatch
{
    public record PostingBatchCreated(DefaultIdType Id, string BatchNumber, DateTime BatchDate, string? Description) : DomainEvent;

    public record PostingBatchUpdated(DefaultIdType Id, string BatchNumber, DateTime BatchDate) : DomainEvent;

    public record PostingBatchDeleted(DefaultIdType Id) : DomainEvent;

    public record PostingBatchPosted(DefaultIdType Id, string BatchNumber, DateTime BatchDate) : DomainEvent;
    public record PostingBatchReversed(DefaultIdType Id, string BatchNumber, DateTime BatchDate, string Reason) : DomainEvent;
    public record PostingBatchApproved(DefaultIdType Id, string BatchNumber, string ApprovedBy, DateTime ApprovedDate) : DomainEvent;
    public record PostingBatchRejected(DefaultIdType Id, string BatchNumber, string RejectedBy, DateTime RejectedDate) : DomainEvent;
}
