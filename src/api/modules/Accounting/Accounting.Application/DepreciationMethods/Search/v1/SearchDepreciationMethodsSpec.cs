namespace Accounting.Application.DepreciationMethods.Search.v1;

public sealed class SearchDepreciationMethodsSpec : Specification<DepreciationMethod>
{
    public SearchDepreciationMethodsSpec(SearchDepreciationMethodsRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrWhiteSpace(request.MethodCode))
        {
            Query.Where(dm => dm.MethodCode.Contains(request.MethodCode));
        }

        if (!string.IsNullOrWhiteSpace(request.MethodName))
        {
            Query.Where(dm => dm.Name.Contains(request.MethodName));
        }

        if (request.IsActive.HasValue)
        {
            Query.Where(dm => dm.IsActive == request.IsActive.Value);
        }


        Query.OrderBy(dm => dm.MethodCode);
    }
}

