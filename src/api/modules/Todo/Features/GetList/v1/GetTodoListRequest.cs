using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.GetList.v1;

/// <summary>
/// Query request for retrieving todo items with filtering and pagination.
/// Supports full pagination, sorting, and search capabilities.
/// </summary>
public record GetTodoListRequest(PaginationFilter Filter) : IRequest<PagedList<TodoDto>>;
