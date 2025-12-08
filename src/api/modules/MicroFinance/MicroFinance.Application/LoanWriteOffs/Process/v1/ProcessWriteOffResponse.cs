namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Process.v1;

public sealed record ProcessWriteOffResponse(DefaultIdType Id, string Status, decimal TotalWriteOff);
