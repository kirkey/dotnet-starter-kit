using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Activate.v1;

public sealed record ActivateRiskCategoryCommand(Guid Id) : IRequest<ActivateRiskCategoryResponse>;
