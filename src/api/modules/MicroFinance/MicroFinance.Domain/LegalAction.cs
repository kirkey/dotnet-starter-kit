using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for LegalAction entity.
/// </summary>
public static class LegalActionConstants
{
    /// <summary>Maximum length for case reference. (2^6 = 64)</summary>
    public const int CaseReferenceMaxLength = 64;

    /// <summary>Maximum length for action type. (2^5 = 32)</summary>
    public const int ActionTypeMaxLength = 32;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for court name. (2^8 = 256)</summary>
    public const int CourtNameMaxLength = 256;

    /// <summary>Maximum length for lawyer name. (2^8 = 256)</summary>
    public const int LawyerNameMaxLength = 256;

    /// <summary>Maximum length for judgment summary. (2^11 = 2048)</summary>
    public const int JudgmentSummaryMaxLength = 2048;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;
}

/// <summary>
/// Represents legal action taken against a defaulting borrower.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track legal proceedings for loan recovery.
/// - Record court filings, hearings, and judgments.
/// - Monitor legal costs and recovery outcomes.
/// - Manage collateral seizure and sale processes.
/// - Support compliance reporting on legal recoveries.
/// 
/// Default values and constraints:
/// - CaseReference: required unique identifier, max 64 characters (example: "LEG-2025-001234")
/// - ActionType: DemandLetter, CivilSuit, Arbitration, Garnishment, CollateralSeizure
/// - Status: Filed by default (Filed, Hearing, Judgment, Execution, Closed)
/// - CourtName: max 256 characters
/// - LawyerName: max 256 characters
/// - JudgmentSummary: max 2048 characters
/// 
/// Business rules:
/// - Legal action requires management approval.
/// - All legal costs tracked for recovery calculations.
/// - Judgment amounts may include principal, interest, and legal fees.
/// - Collateral sale proceeds applied to outstanding balance.
/// - Full audit trail maintained for regulatory compliance.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="CollectionCase"/>
/// <seealso cref="LoanCollateral"/>
public class LegalAction : AuditableEntity, IAggregateRoot
{
    // Action Types
    /// <summary>Demand letter before legal action.</summary>
    public const string TypeDemandLetter = "DEMAND_LETTER";
    /// <summary>Civil suit filed in court.</summary>
    public const string TypeCivilSuit = "CIVIL_SUIT";
    /// <summary>Arbitration proceeding.</summary>
    public const string TypeArbitration = "ARBITRATION";
    /// <summary>Collateral seizure action.</summary>
    public const string TypeCollateralSeizure = "COLLATERAL_SEIZURE";
    /// <summary>Bankruptcy/insolvency proceeding.</summary>
    public const string TypeBankruptcy = "BANKRUPTCY";
    /// <summary>Execution of judgment.</summary>
    public const string TypeExecution = "EXECUTION";
    /// <summary>Garnishment of wages/accounts.</summary>
    public const string TypeGarnishment = "GARNISHMENT";

    // Statuses
    /// <summary>Legal action initiated.</summary>
    public const string StatusInitiated = "INITIATED";
    /// <summary>Case filed with court.</summary>
    public const string StatusFiled = "FILED";
    /// <summary>Case is in court proceedings.</summary>
    public const string StatusInCourt = "IN_COURT";
    /// <summary>Hearing scheduled.</summary>
    public const string StatusHearingScheduled = "HEARING_SCHEDULED";
    /// <summary>Judgment received.</summary>
    public const string StatusJudgment = "JUDGMENT";
    /// <summary>Judgment in favor of MFI.</summary>
    public const string StatusJudgmentWon = "JUDGMENT_WON";
    /// <summary>Judgment against MFI.</summary>
    public const string StatusJudgmentLost = "JUDGMENT_LOST";
    /// <summary>Execution in progress.</summary>
    public const string StatusExecuting = "EXECUTING";
    /// <summary>Case settled out of court.</summary>
    public const string StatusSettled = "SETTLED";
    /// <summary>Case closed/concluded.</summary>
    public const string StatusClosed = "CLOSED";
    /// <summary>Case withdrawn.</summary>
    public const string StatusWithdrawn = "WITHDRAWN";

    /// <summary>Gets the collection case ID.</summary>
    public DefaultIdType CollectionCaseId { get; private set; }

    /// <summary>Gets the collection case navigation property.</summary>
    public virtual CollectionCase? CollectionCase { get; private set; }

    /// <summary>Gets the loan ID.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the member ID (defendant).</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the court/legal reference number.</summary>
    public string? CaseReference { get; private set; }

    /// <summary>Gets the type of legal action.</summary>
    public string ActionType { get; private set; } = default!;

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the date legal action was initiated.</summary>
    public DateOnly InitiatedDate { get; private set; }

    /// <summary>Gets the date case was filed.</summary>
    public DateOnly? FiledDate { get; private set; }

    /// <summary>Gets the next hearing date.</summary>
    public DateOnly? NextHearingDate { get; private set; }

    /// <summary>Gets the judgment date.</summary>
    public DateOnly? JudgmentDate { get; private set; }

    /// <summary>Gets the case closure date.</summary>
    public DateOnly? ClosedDate { get; private set; }

    /// <summary>Gets the court name.</summary>
    public string? CourtName { get; private set; }

    /// <summary>Gets the assigned lawyer/legal counsel name.</summary>
    public string? LawyerName { get; private set; }

    /// <summary>Gets the claim amount.</summary>
    public decimal ClaimAmount { get; private set; }

    /// <summary>Gets the judgment amount (if favorable).</summary>
    public decimal? JudgmentAmount { get; private set; }

    /// <summary>Gets the amount recovered through legal action.</summary>
    public decimal AmountRecovered { get; private set; }

    /// <summary>Gets the legal costs incurred.</summary>
    public decimal LegalCosts { get; private set; }

    /// <summary>Gets the court fees paid.</summary>
    public decimal CourtFees { get; private set; }

    /// <summary>Gets the judgment summary.</summary>
    public string? JudgmentSummary { get; private set; }

    private LegalAction() { }

    private LegalAction(
        DefaultIdType id,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        string actionType,
        decimal claimAmount)
    {
        Id = id;
        CollectionCaseId = collectionCaseId;
        LoanId = loanId;
        MemberId = memberId;
        ActionType = actionType;
        ClaimAmount = claimAmount;
        InitiatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusInitiated;
        AmountRecovered = 0;
        LegalCosts = 0;
        CourtFees = 0;

        QueueDomainEvent(new LegalActionInitiated { LegalAction = this });
    }

    /// <summary>Creates a new LegalAction.</summary>
    public static LegalAction Create(
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        string actionType,
        decimal claimAmount)
    {
        return new LegalAction(
            DefaultIdType.NewGuid(),
            collectionCaseId,
            loanId,
            memberId,
            actionType,
            claimAmount);
    }

    /// <summary>Records case filing.</summary>
    public LegalAction FileCase(
        DateOnly filedDate,
        string caseReference,
        string courtName,
        decimal courtFees)
    {
        FiledDate = filedDate;
        CaseReference = caseReference?.Trim();
        CourtName = courtName?.Trim();
        CourtFees = courtFees;
        Status = StatusFiled;
        return this;
    }

    /// <summary>Assigns a lawyer to the case.</summary>
    public LegalAction AssignLawyer(string lawyerName)
    {
        LawyerName = lawyerName?.Trim();
        return this;
    }

    /// <summary>Schedules a hearing.</summary>
    public LegalAction ScheduleHearing(DateOnly hearingDate)
    {
        NextHearingDate = hearingDate;
        Status = StatusHearingScheduled;
        return this;
    }

    /// <summary>Records a judgment.</summary>
    public LegalAction RecordJudgment(
        DateOnly judgmentDate,
        bool inFavor,
        decimal? judgmentAmount,
        string summary)
    {
        JudgmentDate = judgmentDate;
        JudgmentAmount = judgmentAmount;
        JudgmentSummary = summary?.Trim();
        Status = inFavor ? StatusJudgmentWon : StatusJudgmentLost;
        
        if (inFavor)
        {
            QueueDomainEvent(new LegalActionJudgmentWon { LegalActionId = Id, JudgmentAmount = judgmentAmount ?? 0 });
        }
        return this;
    }

    /// <summary>Records a recovery from legal action.</summary>
    public LegalAction RecordRecovery(decimal amount)
    {
        AmountRecovered += amount;
        return this;
    }

    /// <summary>Records legal costs.</summary>
    public LegalAction AddLegalCosts(decimal amount, string description)
    {
        LegalCosts += amount;
        Notes = string.IsNullOrEmpty(Notes) 
            ? $"Costs: {description} - {amount:C}" 
            : $"{Notes}\nCosts: {description} - {amount:C}";
        return this;
    }

    /// <summary>Settles the case out of court.</summary>
    public LegalAction Settle(decimal settlementAmount, string terms)
    {
        Status = StatusSettled;
        AmountRecovered = settlementAmount;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Notes = $"Settled for {settlementAmount:C}. Terms: {terms}";
        return this;
    }

    /// <summary>Closes the legal action.</summary>
    public LegalAction Close(string reason)
    {
        Status = StatusClosed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Notes = $"Closed: {reason}";
        return this;
    }

    /// <summary>Withdraws the legal action.</summary>
    public LegalAction Withdraw(string reason)
    {
        Status = StatusWithdrawn;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Notes = $"Withdrawn: {reason}";
        return this;
    }
}
