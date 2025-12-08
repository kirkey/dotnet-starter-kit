namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;

public sealed record DisburseTrancheResponse(DefaultIdType Id, string Status, decimal NetAmount, DateOnly DisbursedDate);
