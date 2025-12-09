using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a communication template (SMS, Email, Push, etc.).
/// Provides standardized, parameterized messaging with support for multi-language content.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define reusable message templates for common communications.
/// - Support placeholders for dynamic content (member name, amount, date).
/// - Enable consistent branding and messaging across channels.
/// - Manage templates for different event triggers.
/// - Support regulatory-compliant messaging formats.
/// 
/// Default values and constraints:
/// - Code: Unique template identifier (max 32 chars).
/// - Name: Display name for the template (max 128 chars).
/// - Channel: SMS, Email, Push, WhatsApp (max 32 chars).
/// - Category: LoanReminder, TransactionAlert, AccountUpdate, Marketing (max 64 chars).
/// - Subject: Email subject template (max 256 chars).
/// - Body: Message body with placeholders (max 16384 chars).
/// - Notes: Template usage notes (max 4096 chars).
/// 
/// Business rules:
/// - Ensures consistent, professional messaging.
/// - Efficiency: Pre-defined templates reduce manual effort.
/// - Compliance: Approved templates meet regulatory requirements.
/// - Personalization: Placeholders like {{MemberName}}, {{LoanAmount}}.
/// - Triggers: Auto-send based on events (loan approval, payment due).
/// </remarks>
/// <seealso cref="CommunicationLog"/>
public sealed class CommunicationTemplate : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int Name = 128;
        public const int Channel = 32;
        public const int Category = 64;
        public const int Subject = 256;
        public const int Body = 16384;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Communication channel.
    /// </summary>
    public const string ChannelSms = "SMS";
    public const string ChannelEmail = "Email";
    public const string ChannelPush = "Push";
    public const string ChannelWhatsApp = "WhatsApp";
    public const string ChannelLetter = "Letter";

    /// <summary>
    /// Template category.
    /// </summary>
    public const string CategoryLoan = "Loan";
    public const string CategorySavings = "Savings";
    public const string CategoryReminder = "Reminder";
    public const string CategoryAlert = "Alert";
    public const string CategoryMarketing = "Marketing";
    public const string CategoryOtp = "OTP";
    public const string CategoryWelcome = "Welcome";
    public const string CategoryCollection = "Collection";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDraft = "Draft";

    /// <summary>
    /// Unique template code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Communication channel.
    /// </summary>
    public string Channel { get; private set; } = ChannelSms;

    /// <summary>
    /// Template category.
    /// </summary>
    public string Category { get; private set; } = CategoryLoan;

    /// <summary>
    /// Subject line (for email).
    /// </summary>
    public string? Subject { get; private set; }

    /// <summary>
    /// Message body with placeholders.
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// Available placeholders (JSON array).
    /// </summary>
    public string? Placeholders { get; private set; }

    /// <summary>
    /// Default language.
    /// </summary>
    public string Language { get; private set; } = "en";

    /// <summary>
    /// Whether template requires approval before sending.
    /// </summary>
    public bool RequiresApproval { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    // Navigation properties
    public ICollection<CommunicationLog> Communications { get; private set; } = new List<CommunicationLog>();

    private CommunicationTemplate() { }

    /// <summary>
    /// Creates a new communication template.
    /// </summary>
    public static CommunicationTemplate Create(
        string code,
        string name,
        string channel,
        string category,
        string body,
        string? subject = null,
        string? placeholders = null,
        string language = "en",
        bool requiresApproval = false)
    {
        var template = new CommunicationTemplate
        {
            Code = code,
            Name = name,
            Channel = channel,
            Category = category,
            Body = body,
            Subject = subject,
            Placeholders = placeholders,
            Language = language,
            RequiresApproval = requiresApproval,
            Status = StatusDraft
        };

        template.QueueDomainEvent(new CommunicationTemplateCreated(template));
        return template;
    }

    /// <summary>
    /// Updates the template.
    /// </summary>
    public CommunicationTemplate Update(
        string? name,
        string? subject,
        string? body,
        string? placeholders,
        bool? requiresApproval,
        string? notes)
    {
        if (name is not null) Name = name;
        if (subject is not null) Subject = subject;
        if (body is not null) Body = body;
        if (placeholders is not null) Placeholders = placeholders;
        if (requiresApproval.HasValue) RequiresApproval = requiresApproval.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new CommunicationTemplateUpdated(this));
        return this;
    }

    /// <summary>
    /// Activates the template.
    /// </summary>
    public void Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new CommunicationTemplateActivated(Id));
    }

    /// <summary>
    /// Deactivates the template.
    /// </summary>
    public void Deactivate()
    {
        Status = StatusInactive;
        QueueDomainEvent(new CommunicationTemplateDeactivated(Id));
    }
}
