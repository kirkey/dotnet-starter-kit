using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;

public sealed record CreateFixedDepositCommand(
    [property: DefaultValue("FD-000001")] string CertificateNumber,
    Guid MemberId,
    [property: DefaultValue(10000)] decimal PrincipalAmount,
    [property: DefaultValue(8.5)] decimal InterestRate,
    [property: DefaultValue(12)] int TermMonths,
    [property: DefaultValue(null)] Guid? SavingsProductId,
    [property: DefaultValue(null)] Guid? LinkedSavingsAccountId,
    [property: DefaultValue(null)] DateOnly? DepositDate,
    [property: DefaultValue("TransferToSavings")] string? MaturityInstruction,
    [property: DefaultValue("Fixed deposit account")] string? Notes) : IRequest<CreateFixedDepositResponse>;
