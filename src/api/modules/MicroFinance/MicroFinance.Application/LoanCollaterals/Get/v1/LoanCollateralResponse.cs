namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;

public sealed record LoanCollateralResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    string CollateralType,
    string Description,
    decimal EstimatedValue,
    decimal? ForcedSaleValue,
    DateOnly ValuationDate,
    string? Location,
    string? DocumentReference,
    string Status,
    string? Notes
);
