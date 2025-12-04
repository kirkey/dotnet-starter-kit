namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Process.v1;

public sealed record ProcessWriteOffResponse(Guid Id, string Status, decimal TotalWriteOff);
