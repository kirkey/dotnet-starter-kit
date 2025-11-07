using Accounting.Infrastructure.Endpoints.PostingBatch.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch;

/// <summary>
/// Endpoint configuration for Posting Batch module.
/// </summary>
public static class PostingBatchEndpoints
{
    /// <summary>
    /// Maps all Posting Batch endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPostingBatchEndpoints(this IEndpointRouteBuilder app)
    {
        var postingBatchGroup = app.MapGroup("/posting-batch")
            .WithTags("Posting-Batch")
            .WithDescription("Endpoints for managing posting batch operations");

        // CRUD operations
        postingBatchGroup.MapPostingBatchCreateEndpoint();
        postingBatchGroup.MapPostingBatchGetEndpoint();
        postingBatchGroup.MapPostingBatchSearchEndpoint();

        // Workflow operations
        postingBatchGroup.MapPostingBatchApproveEndpoint();
        postingBatchGroup.MapPostingBatchRejectEndpoint();
        postingBatchGroup.MapPostingBatchPostEndpoint();
        postingBatchGroup.MapPostingBatchReverseEndpoint();

        return app;
    }
}
