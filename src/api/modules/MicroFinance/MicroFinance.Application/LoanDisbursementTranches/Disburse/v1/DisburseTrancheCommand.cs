using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;

public sealed record DisburseTrancheCommand(
    DefaultIdType Id,
    DefaultIdType UserId,
    string ReferenceNumber,
    DateOnly? DisbursedDate = null) : IRequest<DisburseTrancheResponse>;
