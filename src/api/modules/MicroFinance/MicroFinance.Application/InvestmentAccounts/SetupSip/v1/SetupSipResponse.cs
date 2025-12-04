namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

public sealed record SetupSipResponse(
    Guid Id,
    bool HasSip,
    decimal? SipAmount,
    string? SipFrequency,
    DateOnly? NextSipDate);
