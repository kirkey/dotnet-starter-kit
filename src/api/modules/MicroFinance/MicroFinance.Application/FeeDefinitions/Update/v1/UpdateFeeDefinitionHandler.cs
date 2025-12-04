using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Update.v1;

public sealed class UpdateFeeDefinitionHandler(
    [FromKeyedServices("microfinance:feedefinitions")] IRepository<FeeDefinition> repository,
    ILogger<UpdateFeeDefinitionHandler> logger)
    : IRequestHandler<UpdateFeeDefinitionCommand, UpdateFeeDefinitionResponse>
{
    public async Task<UpdateFeeDefinitionResponse> Handle(UpdateFeeDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var feeDefinition = await repository.FirstOrDefaultAsync(
            new FeeDefinitionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (feeDefinition is null)
        {
            throw new NotFoundException($"Fee definition with ID {request.Id} not found.");
        }

        feeDefinition.Update(
            request.Name,
            request.Description,
            request.Amount,
            request.MinAmount,
            request.MaxAmount,
            request.IsTaxable,
            request.TaxRate
        );

        await repository.UpdateAsync(feeDefinition, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Fee definition {FeeDefinitionId} updated", feeDefinition.Id);

        return new UpdateFeeDefinitionResponse(feeDefinition.Id);
    }
}
