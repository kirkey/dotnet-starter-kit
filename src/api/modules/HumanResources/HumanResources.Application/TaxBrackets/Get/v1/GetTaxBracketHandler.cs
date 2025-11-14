using Mapster;

namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;

public sealed class GetTaxBracketHandler(
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<GetTaxBracketRequest, TaxBracketResponse>
{
    public async Task<TaxBracketResponse> Handle(GetTaxBracketRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = bracket ?? throw new TaxBracketNotFoundException(request.Id);

        return bracket.Adapt<TaxBracketResponse>();
    }
}

