using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.RecordPayment.v1;

public sealed record RecordSettlementPaymentCommand(DefaultIdType Id, decimal Amount) : IRequest<RecordSettlementPaymentResponse>;
