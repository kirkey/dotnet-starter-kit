using FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Handler for retrieving a tax bracket by ID.
/// </summary>
public sealed class GetTaxHandler(
    [FromKeyedServices("hr:taxes")] IReadRepository<TaxBracket> repository)
    : IRequestHandler<GetTaxRequest, TaxResponse>
{
    public async Task<TaxResponse> Handle(
        GetTaxRequest request,
        CancellationToken cancellationToken)
    {
        var tax = await repository
            .FirstOrDefaultAsync(new TaxByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (tax is null)
            throw new Exception($"Tax bracket not found: {request.Id}");

        return MapToResponse(tax);
    }

    private static TaxResponse MapToResponse(TaxBracket tax)
    {
        return new TaxResponse
        {
            Id = tax.Id,
            TaxType = tax.TaxType,
            Year = tax.Year,
            MinIncome = tax.MinIncome,
            MaxIncome = tax.MaxIncome,
            Rate = tax.Rate,
            FilingStatus = tax.FilingStatus,
            Description = tax.Description
        };
    }
}

