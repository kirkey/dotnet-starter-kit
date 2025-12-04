using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Deactivate.v1;

public sealed class DeactivateRiskCategoryHandler(
    [FromKeyedServices("microfinance:riskcategories")] IRepository<RiskCategory> repository,
    ILogger<DeactivateRiskCategoryHandler> logger)
    : IRequestHandler<DeactivateRiskCategoryCommand, DeactivateRiskCategoryResponse>
{
    public async Task<DeactivateRiskCategoryResponse> Handle(
        DeactivateRiskCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await repository.FirstOrDefaultAsync(
            new RiskCategoryByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk category {request.Id} not found");

        category.Deactivate();
        await repository.UpdateAsync(category, cancellationToken);

        logger.LogInformation("Risk category deactivated: {CategoryId}", category.Id);

        return new DeactivateRiskCategoryResponse(category.Id, category.Status);
    }
}
