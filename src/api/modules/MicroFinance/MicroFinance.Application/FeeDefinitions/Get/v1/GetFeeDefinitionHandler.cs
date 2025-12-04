using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;

public sealed class GetFeeDefinitionHandler(
    [FromKeyedServices("microfinance:feedefinitions")] IRepository<FeeDefinition> repository)
    : IRequestHandler<GetFeeDefinitionRequest, FeeDefinitionResponse>
{
    public async Task<FeeDefinitionResponse> Handle(GetFeeDefinitionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var feeDefinition = await repository.FirstOrDefaultAsync(
            new FeeDefinitionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (feeDefinition is null)
        {
            throw new NotFoundException($"Fee definition with ID {request.Id} not found.");
        }

        return new FeeDefinitionResponse(
            feeDefinition.Id,
            feeDefinition.Code,
            feeDefinition.Name,
            feeDefinition.Description,
            feeDefinition.FeeType,
            feeDefinition.CalculationType,
            feeDefinition.AppliesTo,
            feeDefinition.ChargeFrequency,
            feeDefinition.Amount,
            feeDefinition.MinAmount,
            feeDefinition.MaxAmount,
            feeDefinition.IsTaxable,
            feeDefinition.TaxRate,
            feeDefinition.IsActive
        );
    }
}
