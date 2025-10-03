using Store.Infrastructure.Endpoints.LotNumbers.v1;

namespace Store.Infrastructure.Endpoints.LotNumbers;

public static class LotNumbersEndpoints
{
    internal static IEndpointRouteBuilder MapLotNumbersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var lotNumbersGroup = endpoints.MapGroup("/lotnumbers")
            .WithTags("LotNumbers")
            .WithDescription("Endpoints for managing lot/batch numbers for inventory traceability");

        lotNumbersGroup.MapCreateLotNumberEndpoint();
        lotNumbersGroup.MapUpdateLotNumberEndpoint();
        lotNumbersGroup.MapDeleteLotNumberEndpoint();
        lotNumbersGroup.MapGetLotNumberEndpoint();
        lotNumbersGroup.MapSearchLotNumbersEndpoint();

        return endpoints;
    }
}
