using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Create.v1;

public sealed class CreateRiskCategoryHandler(
    [FromKeyedServices("microfinance:riskcategories")] IRepository<RiskCategory> repository,
    ILogger<CreateRiskCategoryHandler> logger)
    : IRequestHandler<CreateRiskCategoryCommand, CreateRiskCategoryResponse>
{
    public async Task<CreateRiskCategoryResponse> Handle(
        CreateRiskCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = RiskCategory.Create(
            request.Code,
            request.Name,
            request.RiskType,
            request.Description,
            request.ParentCategoryId,
            request.DefaultSeverity,
            request.WeightFactor,
            request.AlertThreshold,
            request.RequiresEscalation,
            request.EscalationHours,
            request.DisplayOrder);

        await repository.AddAsync(category, cancellationToken);

        logger.LogInformation("Risk category created: {CategoryId}", category.Id);

        return new CreateRiskCategoryResponse(category.Id);
    }
}
