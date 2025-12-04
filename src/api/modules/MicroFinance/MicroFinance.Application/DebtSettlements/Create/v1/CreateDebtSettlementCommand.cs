using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Create.v1;

public sealed record CreateDebtSettlementCommand(
    string ReferenceNumber,
    Guid CollectionCaseId,
    Guid LoanId,
    Guid MemberId,
    string SettlementType,
    decimal OriginalOutstanding,
    decimal SettlementAmount,
    DateOnly DueDate,
    string Terms,
    Guid ProposedById,
    int? NumberOfInstallments = null) : IRequest<CreateDebtSettlementResponse>;
