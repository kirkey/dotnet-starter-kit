namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateDynamic.v1;

public sealed record CreateDynamicQrResponse(DefaultIdType Id, string QrCode, DateTimeOffset ExpiresAt);
