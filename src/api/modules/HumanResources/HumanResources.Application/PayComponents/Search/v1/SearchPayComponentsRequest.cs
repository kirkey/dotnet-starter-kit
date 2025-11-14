namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;

/// <summary>
/// Request to search pay components.
/// </summary>
public sealed record SearchPayComponentsRequest(
    [property: DefaultValue(null)] string? ComponentType = null,
    [property: DefaultValue(true)] bool? IsActive = null,
    [property: DefaultValue(null)] string? SearchTerm = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<PayComponentDto>>;

/// <summary>
/// DTO for pay component search results.
/// </summary>
public sealed record PayComponentDto(
    DefaultIdType Id,
    string ComponentName,
    string ComponentType,
    bool IsActive,
    string? Description);

