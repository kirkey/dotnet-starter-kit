using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed record RecordPremiumPaymentCommand(
    Guid Id,
    DateOnly PaymentDate,
    DateOnly NextDueDate) : IRequest<RecordPremiumPaymentResponse>;
