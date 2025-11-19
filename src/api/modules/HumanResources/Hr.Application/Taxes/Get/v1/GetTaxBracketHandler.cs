namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

using Specifications;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Framework.Core.Persistence;
using Domain.Entities;

/// <summary>
/// Handler for getting tax bracket details.
/// </summary>
public sealed class GetTaxBracketHandler(
    [FromKeyedServices("hr:taxbrackets")] IReadRepository<TaxBracket> repository)
    : IRequestHandler<GetTaxBracketRequest, TaxBracketResponse>
{
    public async Task<TaxBracketResponse> Handle(
        GetTaxBracketRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new TaxBracketByIdSpec(request.Id);
        var bracket = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (bracket is null)
            throw new TaxBracketNotFoundException(request.Id);

        return new TaxBracketResponse(
            bracket.Id,
            bracket.TaxType,
            bracket.Year,
            bracket.MinIncome,
            bracket.MaxIncome,
            bracket.Rate,
            bracket.FilingStatus,
            bracket.Description);
    }
}

