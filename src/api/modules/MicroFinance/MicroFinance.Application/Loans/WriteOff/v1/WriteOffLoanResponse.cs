namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

public sealed record WriteOffLoanResponse(
    Guid Id,
    string Status,
    decimal WrittenOffPrincipal,
    decimal WrittenOffInterest);
