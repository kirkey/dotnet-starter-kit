namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;

using Specifications;

/// <summary>
/// Handler for searching deductions.
/// </summary>
public sealed class SearchDeductionsHandler(
    [FromKeyedServices("hr:deductions")] IReadRepository<Deduction> repository)
    : IRequestHandler<SearchDeductionsRequest, PagedList<DeductionDto>>
{
    public async Task<PagedList<DeductionDto>> Handle(
        SearchDeductionsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDeductionsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var dtos = items.Select(d => new DeductionDto(
            d.Id,
            d.DeductionName,
            d.DeductionType,
            d.RecoveryMethod,
            d.MaxRecoveryPercentage,
            d.RequiresApproval,
            d.IsRecurring,
            d.IsActive)).ToList();

        return new PagedList<DeductionDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

