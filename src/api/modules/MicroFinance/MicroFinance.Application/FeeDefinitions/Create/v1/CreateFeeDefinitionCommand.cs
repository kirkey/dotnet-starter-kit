using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Create.v1;

public sealed record CreateFeeDefinitionCommand(
    [property: DefaultValue("PROC-FEE")] string Code,
    [property: DefaultValue("Processing Fee")] string Name,
    [property: DefaultValue("Processing")] string FeeType,
    [property: DefaultValue("Percentage")] string CalculationType,
    [property: DefaultValue("Loan")] string AppliesTo,
    [property: DefaultValue("OneTime")] string ChargeFrequency,
    [property: DefaultValue(1.5)] decimal Amount,
    [property: DefaultValue("One-time loan processing fee")] string? Description,
    [property: DefaultValue(10)] decimal? MinAmount,
    [property: DefaultValue(500)] decimal? MaxAmount,
    [property: DefaultValue(false)] bool IsTaxable,
    [property: DefaultValue(null)] decimal? TaxRate) : IRequest<CreateFeeDefinitionResponse>;
