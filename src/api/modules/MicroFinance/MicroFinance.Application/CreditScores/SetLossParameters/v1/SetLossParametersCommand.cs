using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.SetLossParameters.v1;

/// <summary>
/// Command to set loss parameters for risk calculation.
/// </summary>
public sealed record SetLossParametersCommand(
    DefaultIdType CreditScoreId,
    decimal? ProbabilityOfDefault,
    decimal? LossGivenDefault,
    decimal? ExposureAtDefault) : IRequest<SetLossParametersResponse>;
