using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.App.Features.GetList.v1;

public record GroupGetListRequest(PaginationFilter Filter) : IRequest<PagedList<GroupDto>>;
