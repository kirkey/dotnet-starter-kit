using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed record RecordCollateralInsurancePremiumCommand(
    Guid Id,
    DateOnly PaymentDate,
    DateOnly NextDueDate) : IRequest<RecordCollateralInsurancePremiumResponse>;
