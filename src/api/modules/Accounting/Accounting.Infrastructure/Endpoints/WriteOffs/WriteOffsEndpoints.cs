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

        // CRUD operations
        group.MapWriteOffCreateEndpoint();
        group.MapWriteOffGetEndpoint();
        group.MapWriteOffUpdateEndpoint();
        group.MapWriteOffSearchEndpoint();

        // Workflow operations
        group.MapWriteOffApproveEndpoint();
        group.MapWriteOffRejectEndpoint();
        group.MapWriteOffPostEndpoint();
        group.MapWriteOffRecordRecoveryEndpoint();
        group.MapWriteOffReverseEndpoint();

        return app;
    }
}

