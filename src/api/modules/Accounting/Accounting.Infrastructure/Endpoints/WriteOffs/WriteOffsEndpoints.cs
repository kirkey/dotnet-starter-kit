using Accounting.Infrastructure.Endpoints.WriteOffs.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs;

public static class WriteOffsEndpoints
{
    internal static IEndpointRouteBuilder MapWriteOffsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/write-offs")
            .WithTags("Write-Offs")
            .WithDescription("Endpoints for managing write-offs")
            .MapToApiVersion(1);

        group.MapWriteOffCreateEndpoint();
        group.MapWriteOffGetEndpoint();
        group.MapWriteOffSearchEndpoint();

        return app;
    }
}

