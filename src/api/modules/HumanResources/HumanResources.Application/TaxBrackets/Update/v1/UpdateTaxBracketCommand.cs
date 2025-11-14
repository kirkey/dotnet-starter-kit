using System.ComponentModel;
using MediatR;
namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Update.v1;
public sealed record UpdateTaxBracketCommand(
    DefaultIdType Id,
    [property: DefaultValue("Single")] string? FilingStatus = null,
    [property: DefaultValue("Updated description")] string? Description = null)
    : IRequest<UpdateTaxBracketResponse>;
