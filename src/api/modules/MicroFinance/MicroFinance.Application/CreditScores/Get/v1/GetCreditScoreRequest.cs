using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Get.v1;

/// <summary>
/// Request to get a credit score by ID.
/// </summary>
public sealed record GetCreditScoreRequest(DefaultIdType Id) : IRequest<CreditScoreResponse>;
