using Mapster;

namespace FSH.Framework.Core.Paging;

public record PagedList<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount) : IPagedList<T>
    where T : class
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public IPagedList<Tr> MapTo<Tr>(Func<T, Tr> map)
        where Tr : class
    {
        return new PagedList<Tr>([.. Items.Select(map)], PageNumber, PageSize, TotalCount);
    }
    public IPagedList<Tr> MapTo<Tr>()
        where Tr : class
    {
        return new PagedList<Tr>(Items.Adapt<IReadOnlyList<Tr>>(), PageNumber, PageSize, TotalCount);
    }
}
