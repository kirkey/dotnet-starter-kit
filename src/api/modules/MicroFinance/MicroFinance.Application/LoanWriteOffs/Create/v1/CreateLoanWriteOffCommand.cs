using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Create.v1;

public sealed record CreateLoanWriteOffCommand(
    Guid LoanId,
    string WriteOffNumber,
    string WriteOffType,
    string Reason,
    decimal PrincipalWriteOff,
    decimal InterestWriteOff,
    decimal PenaltiesWriteOff,
    decimal FeesWriteOff,
    int DaysPastDue,
    int CollectionAttempts = 0) : IRequest<CreateLoanWriteOffResponse>;
