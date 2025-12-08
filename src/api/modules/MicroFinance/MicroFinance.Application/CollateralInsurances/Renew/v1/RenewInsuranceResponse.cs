namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Renew.v1;

public sealed record RenewInsuranceResponse(DefaultIdType Id, DateOnly NewExpiryDate, string Status);
