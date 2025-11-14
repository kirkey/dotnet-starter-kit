namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;

public sealed record TaxBracketResponse(
    DefaultIdType Id,
    string TaxType,
    int Year,
    decimal MinIncome,
    decimal MaxIncome,
    decimal Rate,
    string? FilingStatus,
    string? Description);

