using FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;

using DeductionResponse = DeductionResponse;

/// <summary>
/// Handler for searching deductions.
/// </summary>
public sealed class SearchDeductionsHandler(
    [FromKeyedServices("hr:deductions")] IReadRepository<PayComponent> repository)
    : IRequestHandler<SearchDeductionsRequest, PagedList<DeductionResponse>>
{
    public async Task<PagedList<DeductionResponse>> Handle(
        SearchDeductionsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchDeductionsSpec(request);
        var deductions = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = deductions.Select(MapToResponse).ToList();

        return new PagedList<DeductionResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static DeductionResponse MapToResponse(PayComponent deduction)
    {
        return new DeductionResponse
        {
            Id = deduction.Id,
            ComponentName = deduction.ComponentName,
            ComponentType = deduction.ComponentType,
            GLAccountCode = deduction.GLAccountCode,
            IsActive = deduction.IsActive,
            IsCalculated = deduction.IsCalculated,
            Description = deduction.Description
        };
    }
}

