namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

public sealed record QrPaymentResponse(
    DefaultIdType Id,
    DefaultIdType? WalletId,
    DefaultIdType? MemberId,
    DefaultIdType? AgentId,
    string QrCode,
    string QrType,
    string Status,
    decimal? Amount,
    string? Reference,
    string? Notes,
    int MaxUses,
    int CurrentUses,
    DateTimeOffset GeneratedAt,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? LastUsedAt,
    DefaultIdType? LastTransactionId);
