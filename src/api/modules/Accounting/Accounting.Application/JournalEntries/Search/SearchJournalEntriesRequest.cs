using Accounting.Application.JournalEntries.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.JournalEntries.Search;

public class SearchJournalEntriesRequest : PaginationFilter, IRequest<PagedList<JournalEntryDto>>
{
    public string? Name { get; set; }
    public string? ReferenceNumber { get; set; }
    public bool? IsPosted { get; set; }
}


