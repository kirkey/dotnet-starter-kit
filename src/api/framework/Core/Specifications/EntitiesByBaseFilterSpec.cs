using Ardalis.Specification;
using FSH.Framework.Core.Paging;

namespace FSH.Framework.Core.Specifications;

public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    protected EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
}

public class EntitiesByBaseFilterSpec<T> : Specification<T>
{
    protected EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
}
