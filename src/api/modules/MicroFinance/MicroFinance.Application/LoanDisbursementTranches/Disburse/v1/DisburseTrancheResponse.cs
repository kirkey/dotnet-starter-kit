namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;

public sealed record DisburseTrancheResponse(Guid Id, string Status, decimal NetAmount, DateOnly DisbursedDate);
