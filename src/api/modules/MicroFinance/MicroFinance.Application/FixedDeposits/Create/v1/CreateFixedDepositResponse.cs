namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;

public sealed record CreateFixedDepositResponse(Guid Id, string CertificateNumber, DateOnly MaturityDate);
