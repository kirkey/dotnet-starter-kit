using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;

public sealed record DisburseTrancheCommand(
    Guid Id,
    Guid UserId,
    string ReferenceNumber,
    DateOnly? DisbursedDate = null) : IRequest<DisburseTrancheResponse>;
