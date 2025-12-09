using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a communication (SMS, email, push notification) sent to a member.
/// Tracks delivery status, history, and provides audit trail for all member communications.
/// </summary>
/// <remarks>
/// Use cases:
/// - Send loan repayment reminders via SMS.
/// - Notify members of loan approval/disbursement.
/// - Send account statements and transaction alerts.
/// - Track delivery status and failures for retry.
/// - Maintain communication history for dispute resolution.
/// 
/// Default values and constraints:
/// - Channel: SMS, Email, Push, WhatsApp (max 32 chars).
/// - Recipient: Phone number or email address (max 256 chars).
/// - Subject: Message subject for email (max 256 chars).
/// - Body: Message content (max 16384 chars).
/// - DeliveryStatus: Pending, Sent, Delivered, Failed (max 32 chars).
/// - ErrorMessage: Failure reason if applicable (max 1024 chars).
/// 
/// Business rules:
/// - Effective communication improves repayment rates.
/// - Payment Reminders: Reduce delinquency with timely notifications.
/// - Transaction Alerts: Real-time notifications for security.
/// - SMS: Most common, highest reach in rural areas.
/// - Compliance: Audit trail for regulatory requirements.
/// - Failed messages may be retried automatically.
/// </remarks>
/// <seealso cref="Member"/>
/// <seealso cref="CommunicationTemplate"/>
public sealed class CommunicationLog : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants.
    /// </summary>
    public static class MaxLengths
    {
        public const int Channel = 32;
        public const int Recipient = 256;
        public const int Subject = 256;
        public const int Body = 16384;
        public const int DeliveryStatus = 32;
        public const int ErrorMessage = 1024;
        public const int ExternalId = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Communication channel.
    /// </summary>
    public const string ChannelSms = "SMS";
    public const string ChannelEmail = "Email";
    public const string ChannelPush = "Push";
    public const string ChannelWhatsApp = "WhatsApp";

    /// <summary>
    /// Delivery status values.
    /// </summary>
    public const string StatusQueued = "Queued";
    public const string StatusSending = "Sending";
    public const string StatusSent = "Sent";
    public const string StatusDelivered = "Delivered";
    public const string StatusFailed = "Failed";
    public const string StatusBounced = "Bounced";
    public const string StatusOpened = "Opened";
    public const string StatusClicked = "Clicked";

    /// <summary>
    /// Reference to the template used.
    /// </summary>
    public Guid? TemplateId { get; private set; }

    /// <summary>
    /// Reference to the member.
    /// </summary>
    public Guid? MemberId { get; private set; }

    /// <summary>
    /// Reference to associated loan (if loan-related).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Communication channel.
    /// </summary>
    public string Channel { get; private set; } = ChannelSms;

    /// <summary>
    /// Recipient address (phone, email, etc.).
    /// </summary>
    public string Recipient { get; private set; } = string.Empty;

    /// <summary>
    /// Subject line (for email).
    /// </summary>
    public string? Subject { get; private set; }

    /// <summary>
    /// Message body.
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// Current delivery status.
    /// </summary>
    public string DeliveryStatus { get; private set; } = StatusQueued;

    /// <summary>
    /// Time when message was sent.
    /// </summary>
    public DateTime? SentAt { get; private set; }

    /// <summary>
    /// Time when message was delivered.
    /// </summary>
    public DateTime? DeliveredAt { get; private set; }

    /// <summary>
    /// Time when message was opened (for email).
    /// </summary>
    public DateTime? OpenedAt { get; private set; }

    /// <summary>
    /// Number of retry attempts.
    /// </summary>
    public int RetryCount { get; private set; }

    /// <summary>
    /// Error message (if failed).
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// External provider message ID.
    /// </summary>
    public string? ExternalId { get; private set; }

    /// <summary>
    /// Cost of sending (if applicable).
    /// </summary>
    public decimal? Cost { get; private set; }

    /// <summary>
    /// User who initiated the communication.
    /// </summary>
    public Guid? SentByUserId { get; private set; }

    // Navigation properties
    public CommunicationTemplate? Template { get; private set; }
    public Member? Member { get; private set; }
    public Loan? Loan { get; private set; }

    private CommunicationLog() { }

    /// <summary>
    /// Creates a new communication log entry.
    /// </summary>
    public static CommunicationLog Create(
        string channel,
        string recipient,
        string body,
        Guid? memberId = null,
        Guid? loanId = null,
        Guid? templateId = null,
        string? subject = null,
        Guid? sentByUserId = null)
    {
        var log = new CommunicationLog
        {
            Channel = channel,
            Recipient = recipient,
            Body = body,
            MemberId = memberId,
            LoanId = loanId,
            TemplateId = templateId,
            Subject = subject,
            SentByUserId = sentByUserId,
            DeliveryStatus = StatusQueued,
            RetryCount = 0
        };

        log.QueueDomainEvent(new CommunicationQueued(log));
        return log;
    }

    /// <summary>
    /// Marks as sending.
    /// </summary>
    public void MarkSending()
    {
        DeliveryStatus = StatusSending;
    }

    /// <summary>
    /// Marks as sent successfully.
    /// </summary>
    public void MarkSent(string? externalId = null, decimal? cost = null)
    {
        SentAt = DateTime.UtcNow;
        ExternalId = externalId;
        Cost = cost;
        DeliveryStatus = StatusSent;

        QueueDomainEvent(new CommunicationSent(Id, Recipient));
    }

    /// <summary>
    /// Marks as delivered.
    /// </summary>
    public void MarkDelivered()
    {
        DeliveredAt = DateTime.UtcNow;
        DeliveryStatus = StatusDelivered;

        QueueDomainEvent(new CommunicationDelivered(Id, Recipient));
    }

    /// <summary>
    /// Marks as failed.
    /// </summary>
    public void MarkFailed(string errorMessage)
    {
        ErrorMessage = errorMessage;
        DeliveryStatus = StatusFailed;

        QueueDomainEvent(new CommunicationFailed(Id, Recipient, errorMessage));
    }

    /// <summary>
    /// Marks as bounced.
    /// </summary>
    public void MarkBounced(string? reason = null)
    {
        ErrorMessage = reason;
        DeliveryStatus = StatusBounced;

        QueueDomainEvent(new CommunicationBounced(Id, Recipient, reason));
    }

    /// <summary>
    /// Marks as opened (for email tracking).
    /// </summary>
    public void MarkOpened()
    {
        OpenedAt = DateTime.UtcNow;
        DeliveryStatus = StatusOpened;

        QueueDomainEvent(new CommunicationOpened(Id, Recipient));
    }

    /// <summary>
    /// Increments retry count.
    /// </summary>
    public void IncrementRetry()
    {
        RetryCount++;
        DeliveryStatus = StatusQueued;
    }
}
