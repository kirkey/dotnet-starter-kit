using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace Accounting.Domain
{
    public class PostingBatch : AuditableEntity, IAggregateRoot
    {
        public string BatchNumber { get; private set; }
        public DateTime BatchDate { get; private set; }
        public string Status { get; private set; } // Draft, Posted, Reversed
        // Hide base Description property with 'new' keyword to resolve warning
        public new string? Description { get; private set; }
        public DefaultIdType? PeriodId { get; private set; }
        public string ApprovalStatus { get; private set; } // Pending, Approved, Rejected
        public string? ApprovedBy { get; private set; }
        public DateTime? ApprovedDate { get; private set; }
        private readonly List<JournalEntry> _journalEntries = new();
        public IReadOnlyCollection<JournalEntry> JournalEntries => _journalEntries.AsReadOnly();

        // EF Core requires a parameterless constructor
        private PostingBatch() { }

        private PostingBatch(string batchNumber, DateTime batchDate, string? description = null, DefaultIdType? periodId = null)
        {
            BatchNumber = batchNumber.Trim();
            BatchDate = batchDate;
            Status = "Draft";
            Description = description?.Trim();
            PeriodId = periodId;
            ApprovalStatus = "Pending";
        }

        public static PostingBatch Create(string batchNumber, DateTime batchDate, string? description = null, DefaultIdType? periodId = null)
        {
            if (string.IsNullOrWhiteSpace(batchNumber))
                throw new ArgumentException("Batch number is required.");
            return new PostingBatch(batchNumber, batchDate, description, periodId);
        }

        public void AddJournalEntry(JournalEntry entry)
        {
            if (Status != "Draft")
                throw new InvalidOperationException("Can only add entries to a draft batch.");
            _journalEntries.Add(entry);
        }

        public void Post()
        {
            if (Status != "Draft")
                throw new InvalidOperationException("Only draft batches can be posted.");
            if (ApprovalStatus != "Approved")
                throw new InvalidOperationException("Batch must be approved before posting.");
            foreach (var entry in _journalEntries)
            {
                entry.Post();
            }
            Status = "Posted";
            QueueDomainEvent(new Events.PostingBatch.PostingBatchPosted(Id, BatchNumber, BatchDate));
        }

        public void Reverse(string reason)
        {
            if (Status != "Posted")
                throw new InvalidOperationException("Only posted batches can be reversed.");
            foreach (var entry in _journalEntries)
            {
                entry.Reverse(DateTime.UtcNow, reason);
            }
            Status = "Reversed";
            QueueDomainEvent(new Events.PostingBatch.PostingBatchReversed(Id, BatchNumber, BatchDate, reason));
        }

        public void Approve(string approvedBy)
        {
            if (ApprovalStatus == "Approved")
                throw new InvalidOperationException("Batch already approved.");
            ApprovalStatus = "Approved";
            ApprovedBy = approvedBy;
            ApprovedDate = DateTime.UtcNow;
            QueueDomainEvent(new Events.PostingBatch.PostingBatchApproved(Id, BatchNumber, ApprovedBy, ApprovedDate.Value));
        }

        public void Reject(string rejectedBy)
        {
            if (ApprovalStatus == "Rejected")
                throw new InvalidOperationException("Batch already rejected.");
            ApprovalStatus = "Rejected";
            ApprovedBy = rejectedBy;
            ApprovedDate = DateTime.UtcNow;
            QueueDomainEvent(new Events.PostingBatch.PostingBatchRejected(Id, BatchNumber, ApprovedBy, ApprovedDate.Value));
        }
    }
}
