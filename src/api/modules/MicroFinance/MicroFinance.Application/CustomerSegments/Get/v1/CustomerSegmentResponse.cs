namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;

public sealed record CustomerSegmentResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string? Description,
    string Status,
    string SegmentType,
    string? SegmentCriteria,
    int Priority,
    int MemberCount,
    decimal? MinIncomeLevel,
    decimal? MaxIncomeLevel,
    string? RiskLevel,
    decimal? DefaultInterestModifier,
    decimal? DefaultFeeModifier);
