using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Deactivate.v1;

public sealed record DeactivateRiskCategoryCommand(DefaultIdType Id) : IRequest<DeactivateRiskCategoryResponse>;
