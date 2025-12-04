namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;

public sealed record ShareProductResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    decimal NominalValue,
    decimal CurrentPrice,
    int MinSharesForMembership,
    int? MaxSharesPerMember,
    bool AllowTransfer,
    bool AllowRedemption,
    int? MinHoldingPeriodMonths,
    bool PaysDividends,
    bool IsActive
);
