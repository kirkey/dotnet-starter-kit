using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Search.v1;

public sealed record SearchBranchesCommand(
    string? Code = null,
    string? Name = null,
    string? BranchType = null,
    string? Status = null,
    string? City = null,
    string? State = null,
    Guid? ParentBranchId = null,
    PaginationFilter? Filter = null) : IRequest<PagedList<BranchSummaryResponse>>;
