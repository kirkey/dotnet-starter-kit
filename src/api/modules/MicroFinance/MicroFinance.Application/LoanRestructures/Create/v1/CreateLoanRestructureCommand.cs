using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Create.v1;

public sealed record CreateLoanRestructureCommand(
    DefaultIdType LoanId,
    string RestructureNumber,
    string RestructureType,
    decimal OriginalPrincipal,
    decimal OriginalInterestRate,
    int OriginalRemainingTerm,
    decimal OriginalInstallmentAmount,
    decimal NewPrincipal,
    decimal NewInterestRate,
    int NewTerm,
    decimal NewInstallmentAmount,
    string? Reason = null,
    int GracePeriodMonths = 0,
    decimal WaivedAmount = 0,
    decimal RestructureFee = 0) : IRequest<CreateLoanRestructureResponse>;
