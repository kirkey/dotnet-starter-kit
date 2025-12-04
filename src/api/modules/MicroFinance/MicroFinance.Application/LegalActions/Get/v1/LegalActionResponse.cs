namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;

public sealed record LegalActionResponse(
    Guid Id,
    Guid CollectionCaseId,
    Guid LoanId,
    Guid MemberId,
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
