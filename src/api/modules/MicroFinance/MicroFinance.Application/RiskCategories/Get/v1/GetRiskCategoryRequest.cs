using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;

public sealed record GetRiskCategoryRequest(DefaultIdType Id) : IRequest<RiskCategoryResponse>;
