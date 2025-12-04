using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Submit.v1;

public sealed record SubmitValuationCommand(Guid Id) : IRequest<SubmitValuationResponse>;
