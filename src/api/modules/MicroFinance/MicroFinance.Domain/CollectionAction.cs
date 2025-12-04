using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for CollectionAction entity.
/// </summary>
public static class CollectionActionConstants
{
    /// <summary>Maximum length for action type. (2^5 = 32)</summary>
    public const int ActionTypeMaxLength = 32;

    /// <summary>Maximum length for outcome. (2^6 = 64)</summary>
    public const int OutcomeMaxLength = 64;

    /// <summary>Maximum length for contact method. (2^5 = 32)</summary>
    public const int ContactMethodMaxLength = 32;

    /// <summary>Maximum length for contact person. (2^8 = 256)</summary>
    public const int ContactPersonMaxLength = 256;

    /// <summary>Maximum length for phone number. (2^5 = 32)</summary>
    public const int PhoneNumberMaxLength = 32;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;
}

/// <summary>
/// Represents a collection action taken on a delinquent loan.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record phone calls made to delinquent borrowers</description></item>
///   <item><description>Log SMS and email reminders sent</description></item>
///   <item><description>Track field visits for in-person collection</description></item>
///   <item><description>Document letters and notices sent</description></item>
///   <item><description>Record outcomes of each collection attempt</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Collection actions form the audit trail of recovery efforts on delinquent loans.
/// Each action records what was done, when, by whom, and the outcome.
/// This supports regulatory compliance and collection performance analysis.
/// </para>
/// </remarks>
public class CollectionAction : AuditableEntity, IAggregateRoot
{
    // Action Types
    /// <summary>Phone call to borrower.</summary>
    public const string TypePhoneCall = "PHONE_CALL";
    /// <summary>SMS message sent.</summary>
    public const string TypeSms = "SMS";
    /// <summary>Email sent.</summary>
    public const string TypeEmail = "EMAIL";
    /// <summary>Field visit to borrower.</summary>
    public const string TypeFieldVisit = "FIELD_VISIT";
    /// <summary>Formal letter sent.</summary>
    public const string TypeLetter = "LETTER";
    /// <summary>Demand notice sent.</summary>
    public const string TypeDemandNotice = "DEMAND_NOTICE";
    /// <summary>Legal notice served.</summary>
    public const string TypeLegalNotice = "LEGAL_NOTICE";
    /// <summary>Contact with guarantor.</summary>
    public const string TypeGuarantorContact = "GUARANTOR_CONTACT";
    /// <summary>Contact with group leader.</summary>
    public const string TypeGroupLeaderContact = "GROUP_LEADER";
    /// <summary>WhatsApp or other messaging.</summary>
    public const string TypeMessaging = "MESSAGING";

    // Outcomes
    /// <summary>Borrower contacted successfully.</summary>
    public const string OutcomeContacted = "CONTACTED";
    /// <summary>No answer/response.</summary>
    public const string OutcomeNoAnswer = "NO_ANSWER";
    /// <summary>Phone unreachable.</summary>
    public const string OutcomeUnreachable = "UNREACHABLE";
    /// <summary>Wrong number/contact.</summary>
    public const string OutcomeWrongContact = "WRONG_CONTACT";
    /// <summary>Borrower promised to pay.</summary>
    public const string OutcomePromisedToPay = "PROMISED_TO_PAY";
    /// <summary>Payment received during action.</summary>
    public const string OutcomePaymentReceived = "PAYMENT_RECEIVED";
    /// <summary>Borrower refused to pay.</summary>
    public const string OutcomeRefused = "REFUSED";
    /// <summary>Borrower not found at address.</summary>
    public const string OutcomeNotFound = "NOT_FOUND";
    /// <summary>Message/letter delivered.</summary>
    public const string OutcomeDelivered = "DELIVERED";
    /// <summary>Rescheduled for follow-up.</summary>
    public const string OutcomeRescheduled = "RESCHEDULED";

    /// <summary>Gets the collection case ID.</summary>
    public DefaultIdType CollectionCaseId { get; private set; }

    /// <summary>Gets the collection case navigation property.</summary>
    public virtual CollectionCase? CollectionCase { get; private set; }

    /// <summary>Gets the loan ID for direct reference.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the type of collection action.</summary>
    public string ActionType { get; private set; } = default!;

    /// <summary>Gets the date and time of the action.</summary>
    public DateTime ActionDateTime { get; private set; }

    /// <summary>Gets the staff ID who performed the action.</summary>
    public DefaultIdType PerformedById { get; private set; }

    /// <summary>Gets the contact method used.</summary>
    public string? ContactMethod { get; private set; }

    /// <summary>Gets the phone number called (if applicable).</summary>
    public string? PhoneNumberCalled { get; private set; }

    /// <summary>Gets the name of person contacted.</summary>
    public string? ContactPerson { get; private set; }

    /// <summary>Gets the outcome of the action.</summary>
    public string Outcome { get; private set; } = default!;

    /// <summary>Gets the description of what occurred.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the promised payment amount (if applicable).</summary>
    public decimal? PromisedAmount { get; private set; }

    /// <summary>Gets the promised payment date (if applicable).</summary>
    public DateOnly? PromisedDate { get; private set; }

    /// <summary>Gets the next scheduled follow-up date.</summary>
    public DateOnly? NextFollowUpDate { get; private set; }

    /// <summary>Gets the duration of the contact in minutes.</summary>
    public int? DurationMinutes { get; private set; }

    /// <summary>Gets GPS latitude for field visits.</summary>
    public decimal? Latitude { get; private set; }

    /// <summary>Gets GPS longitude for field visits.</summary>
    public decimal? Longitude { get; private set; }

    private CollectionAction() { }

    private CollectionAction(
        DefaultIdType id,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        string actionType,
        DefaultIdType performedById,
        string outcome,
        string? description)
    {
        Id = id;
        CollectionCaseId = collectionCaseId;
        LoanId = loanId;
        ActionType = actionType;
        ActionDateTime = DateTime.UtcNow;
        PerformedById = performedById;
        Outcome = outcome;
        Description = description?.Trim();

        QueueDomainEvent(new CollectionActionRecorded { CollectionAction = this });
    }

    /// <summary>Creates a new CollectionAction.</summary>
    public static CollectionAction Create(
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        string actionType,
        DefaultIdType performedById,
        string outcome,
        string? description = null)
    {
        return new CollectionAction(
            DefaultIdType.NewGuid(),
            collectionCaseId,
            loanId,
            actionType,
            performedById,
            outcome,
            description);
    }

    /// <summary>Records phone call details.</summary>
    public CollectionAction WithPhoneCallDetails(
        string phoneNumber,
        string? contactPerson,
        int durationMinutes)
    {
        PhoneNumberCalled = phoneNumber?.Trim();
        ContactPerson = contactPerson?.Trim();
        DurationMinutes = durationMinutes;
        ContactMethod = "PHONE";
        return this;
    }

    /// <summary>Records field visit details with GPS location.</summary>
    public CollectionAction WithFieldVisitDetails(
        decimal latitude,
        decimal longitude,
        string? contactPerson)
    {
        Latitude = latitude;
        Longitude = longitude;
        ContactPerson = contactPerson?.Trim();
        ContactMethod = "IN_PERSON";
        return this;
    }

    /// <summary>Records a promise to pay.</summary>
    public CollectionAction WithPromiseToPay(decimal amount, DateOnly promisedDate)
    {
        PromisedAmount = amount;
        PromisedDate = promisedDate;
        Outcome = OutcomePromisedToPay;
        return this;
    }

    /// <summary>Sets the next follow-up date.</summary>
    public CollectionAction WithFollowUp(DateOnly followUpDate)
    {
        NextFollowUpDate = followUpDate;
        return this;
    }

    /// <summary>Adds notes to the action.</summary>
    public CollectionAction WithNotes(string notes)
    {
        Notes = notes?.Trim();
        return this;
    }
}
