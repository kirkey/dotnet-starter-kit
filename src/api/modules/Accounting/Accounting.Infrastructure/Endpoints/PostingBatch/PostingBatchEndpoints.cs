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

        // Version 1 endpoints will be added here when implemented
        // postingBatchGroup.MapPostingBatchCreateEndpoint();
        // postingBatchGroup.MapPostingBatchUpdateEndpoint();
        // postingBatchGroup.MapPostingBatchDeleteEndpoint();
        // postingBatchGroup.MapPostingBatchGetEndpoint();
        // postingBatchGroup.MapPostingBatchSearchEndpoint();

        return app;
    }
}
