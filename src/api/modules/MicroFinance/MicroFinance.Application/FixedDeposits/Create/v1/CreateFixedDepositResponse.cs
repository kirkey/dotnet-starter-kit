namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;

public sealed record CreateFixedDepositResponse(DefaultIdType Id, string CertificateNumber, DateOnly MaturityDate);
