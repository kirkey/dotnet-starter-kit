namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.CreditFloat.v1;

public sealed record CreditFloatResponse(Guid Id, decimal Amount, decimal NewBalance);
