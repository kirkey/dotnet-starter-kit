using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Create.v1;

public sealed record CreateLoanDisbursementTrancheCommand(
    Guid LoanId,
    int TrancheSequence,
    string TrancheNumber,
    DateOnly ScheduledDate,
    decimal Amount,
    string DisbursementMethod,
    string? Milestone = null,
    string? BankAccountNumber = null,
    string? BankName = null,
    decimal Deductions = 0) : IRequest<CreateLoanDisbursementTrancheResponse>;
