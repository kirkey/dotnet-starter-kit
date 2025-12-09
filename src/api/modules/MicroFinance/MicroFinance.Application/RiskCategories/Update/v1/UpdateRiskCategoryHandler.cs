using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Update.v1;

/// <summary>
/// Handler for updating a risk category.
/// </summary>
public sealed class UpdateRiskCategoryHandler(
    ILogger<UpdateRiskCategoryHandler> logger,
    [FromKeyedServices("microfinance:riskcategories")] IRepository<RiskCategory> repository)
    : IRequestHandler<UpdateRiskCategoryCommand, UpdateRiskCategoryResponse>
{
    public async Task<UpdateRiskCategoryResponse> Handle(UpdateRiskCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var riskCategory = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (riskCategory is null)
        {
            throw new NotFoundException($"Risk category with ID {request.Id} not found.");
        }

        riskCategory.Update(
            name: request.Name,
            description: request.Description,
            defaultSeverity: request.DefaultSeverity,
            weightFactor: request.WeightFactor,
            alertThreshold: request.AlertThreshold,
            requiresEscalation: request.RequiresEscalation,
            escalationHours: request.EscalationHours,
            displayOrder: request.DisplayOrder,
            notes: request.Notes);

        await repository.UpdateAsync(riskCategory, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Risk category updated with ID: {Id}, Name: {Name}", riskCategory.Id, riskCategory.Name);

        return new UpdateRiskCategoryResponse(riskCategory.Id);
    }
}
