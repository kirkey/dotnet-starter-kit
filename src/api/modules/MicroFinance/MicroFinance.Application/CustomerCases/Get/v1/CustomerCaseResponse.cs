namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Get.v1;

/// <summary>
/// Response containing customer case details.
/// </summary>
public sealed record CustomerCaseResponse(
    DefaultIdType Id,
    string CaseNumber,
    DefaultIdType MemberId,
    string Subject,
    string Category,
    string Priority,
    string Status,
    string Description,
    string Channel,
    DefaultIdType? AssignedToId,
    DateTimeOffset OpenedAt,
    DateTimeOffset? FirstResponseAt,
    DateTimeOffset? ResolvedAt,
    DateTimeOffset? ClosedAt,
    string? Resolution,
    int EscalationLevel,
    bool SlaBreached,
    int? CustomerSatisfactionScore);
