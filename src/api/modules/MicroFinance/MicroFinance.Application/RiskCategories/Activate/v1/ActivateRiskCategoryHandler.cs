using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Activate.v1;

public sealed class ActivateRiskCategoryHandler(
    [FromKeyedServices("microfinance:riskcategories")] IRepository<RiskCategory> repository,
    ILogger<ActivateRiskCategoryHandler> logger)
    : IRequestHandler<ActivateRiskCategoryCommand, ActivateRiskCategoryResponse>
{
    public async Task<ActivateRiskCategoryResponse> Handle(
        ActivateRiskCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await repository.FirstOrDefaultAsync(
            new RiskCategoryByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk category {request.Id} not found");

        category.Activate();
        await repository.UpdateAsync(category, cancellationToken);

        logger.LogInformation("Risk category activated: {CategoryId}", category.Id);

        return new ActivateRiskCategoryResponse(category.Id, category.Status);
    }
}
