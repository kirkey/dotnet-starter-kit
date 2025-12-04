using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Create.v1;

public sealed class CreateFeeDefinitionHandler(
    [FromKeyedServices("microfinance:feedefinitions")] IRepository<FeeDefinition> repository,
    ILogger<CreateFeeDefinitionHandler> logger)
    : IRequestHandler<CreateFeeDefinitionCommand, CreateFeeDefinitionResponse>
{
    public async Task<CreateFeeDefinitionResponse> Handle(CreateFeeDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate fee code
        var existingFee = await repository.FirstOrDefaultAsync(
            new FeeDefinitionByCodeSpec(request.Code), cancellationToken).ConfigureAwait(false);

        if (existingFee is not null)
        {
            throw new InvalidOperationException($"A fee definition with code '{request.Code}' already exists.");
        }

        var feeDefinition = FeeDefinition.Create(
            request.Code,
            request.Name,
            request.FeeType,
            request.CalculationType,
            request.AppliesTo,
            request.ChargeFrequency,
            request.Amount,
            request.Description,
            request.MinAmount,
            request.MaxAmount,
            request.IsTaxable,
            request.TaxRate
        );

        await repository.AddAsync(feeDefinition, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Fee definition {Code} created with ID {FeeDefinitionId}", feeDefinition.Code, feeDefinition.Id);

        return new CreateFeeDefinitionResponse(feeDefinition.Id, feeDefinition.Code);
    }
}
