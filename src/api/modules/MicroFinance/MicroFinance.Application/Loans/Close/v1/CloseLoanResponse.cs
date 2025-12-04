namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;

public sealed record CloseLoanResponse(Guid Id, string Status, DateOnly CloseDate);
