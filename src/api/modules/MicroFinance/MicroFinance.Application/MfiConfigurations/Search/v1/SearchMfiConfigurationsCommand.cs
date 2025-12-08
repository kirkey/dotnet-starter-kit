using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Search.v1;

public class SearchMfiConfigurationsCommand : PaginationFilter, IRequest<PagedList<MfiConfigurationSummaryResponse>>
{
    public string? Key { get; set; }
    public string? Category { get; set; }
    public string? DataType { get; set; }
    public bool? IsEditable { get; set; }
    public bool? IsEncrypted { get; set; }
    public DefaultIdType? BranchId { get; set; }
}

public sealed record MfiConfigurationSummaryResponse(
    DefaultIdType Id,
    string Key,
    string Value,
    string Category,
    string DataType,
    string? Description,
    bool IsEditable,
    bool IsEncrypted,
    DefaultIdType? BranchId,
    int DisplayOrder);
