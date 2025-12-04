using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;

public sealed class GetRiskCategoryHandler(
    [FromKeyedServices("microfinance:riskcategories")] IReadRepository<RiskCategory> repository)
    : IRequestHandler<GetRiskCategoryRequest, RiskCategoryResponse>
{
    public async Task<RiskCategoryResponse> Handle(
        GetRiskCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = await repository.FirstOrDefaultAsync(
            new RiskCategoryByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk category {request.Id} not found");

        return new RiskCategoryResponse(
            category.Id,
            category.Code,
            category.Name,
            category.Description,
            category.RiskType,
            category.ParentCategoryId,
            category.DefaultSeverity,
            category.WeightFactor,
            category.AlertThreshold,
            category.RequiresEscalation,
            category.EscalationHours,
            category.DisplayOrder,
            category.Status);
    }
}
