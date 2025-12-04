namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.SetLossParameters.v1;

/// <summary>
/// Response after setting loss parameters.
/// </summary>
public sealed record SetLossParametersResponse(
    Guid Id,
    decimal? ExpectedLoss);
