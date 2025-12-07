namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed record RecordCollateralInsurancePremiumResponse(Guid Id, DateOnly LastPremiumPaidDate, DateOnly NextPremiumDueDate);
