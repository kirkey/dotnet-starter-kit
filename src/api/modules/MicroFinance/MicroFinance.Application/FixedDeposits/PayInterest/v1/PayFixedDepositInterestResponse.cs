namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PayInterest.v1;

/// <summary>
/// Response after paying interest.
/// </summary>
public sealed record PayFixedDepositInterestResponse(
    DefaultIdType DepositId,
    decimal AmountPaid,
    decimal TotalInterestPaid,
    string Message);
