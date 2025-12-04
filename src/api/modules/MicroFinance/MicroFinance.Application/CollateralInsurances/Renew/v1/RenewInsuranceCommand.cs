using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Renew.v1;

public sealed record RenewInsuranceCommand(
    Guid Id,
    DateOnly NewExpiryDate,
    decimal? NewPremium = null) : IRequest<RenewInsuranceResponse>;
