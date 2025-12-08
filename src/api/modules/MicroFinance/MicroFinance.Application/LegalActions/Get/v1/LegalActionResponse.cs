namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;

public sealed record LegalActionResponse(
    DefaultIdType Id,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    string? CaseReference,
    string ActionType,
    string Status,
    DateOnly InitiatedDate,
    DateOnly? FiledDate,
    DateOnly? NextHearingDate,
    DateOnly? JudgmentDate,
    DateOnly? ClosedDate,
    string? CourtName,
    string? LawyerName,
    decimal ClaimAmount,
    decimal? JudgmentAmount,
    decimal AmountRecovered,
    decimal LegalCosts,
    decimal CourtFees,
    string? JudgmentSummary);
