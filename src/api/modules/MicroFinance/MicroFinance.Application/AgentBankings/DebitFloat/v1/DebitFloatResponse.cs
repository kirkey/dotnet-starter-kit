namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.DebitFloat.v1;

public sealed record DebitFloatResponse(Guid Id, decimal Amount, decimal NewBalance);
