using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Update.v1;

public sealed record UpdateFeeDefinitionCommand(
    DefaultIdType Id,
    [property: DefaultValue("Updated Processing Fee")] string? Name,
    [property: DefaultValue("Updated description")] string? Description,
    [property: DefaultValue(2.0)] decimal? Amount,
    [property: DefaultValue(15)] decimal? MinAmount,
    [property: DefaultValue(600)] decimal? MaxAmount,
    [property: DefaultValue(true)] bool? IsTaxable,
    [property: DefaultValue(5)] decimal? TaxRate) : IRequest<UpdateFeeDefinitionResponse>;
