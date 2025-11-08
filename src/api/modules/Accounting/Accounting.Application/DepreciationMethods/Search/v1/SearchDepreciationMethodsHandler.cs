using Accounting.Application.DepreciationMethods.Responses;

namespace Accounting.Application.DepreciationMethods.Search.v1;

public sealed class SearchDepreciationMethodsHandler(
    IReadRepository<DepreciationMethod> repository,
    ILogger<SearchDepreciationMethodsHandler> logger)
    : IRequestHandler<SearchDepreciationMethodsRequest, PagedList<DepreciationMethodResponse>>
{
    private readonly IReadRepository<DepreciationMethod> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<SearchDepreciationMethodsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PagedList<DepreciationMethodResponse>> Handle(SearchDepreciationMethodsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Searching depreciation methods with filters");

        var spec = new SearchDepreciationMethodsSpec(request);
        var methods = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = methods.Select(dm => new DepreciationMethodResponse(
            dm.MethodCode,
            dm.CalculationFormula,
            dm.IsActive)
        {
            Name = dm.Name,
            Description = dm.Description
        }).ToList();

        _logger.LogInformation("Found {Count} depreciation methods", response.Count);

        return new PagedList<DepreciationMethodResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}

