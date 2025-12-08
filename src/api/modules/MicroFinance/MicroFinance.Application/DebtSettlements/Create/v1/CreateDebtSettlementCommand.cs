using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Create.v1;

public sealed record CreateDebtSettlementCommand(
    string ReferenceNumber,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    string SettlementType,
    decimal OriginalOutstanding,
    decimal SettlementAmount,
    DateOnly DueDate,
    string Terms,
    DefaultIdType ProposedById,
    int? NumberOfInstallments = null) : IRequest<CreateDebtSettlementResponse>;
