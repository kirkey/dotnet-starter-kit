using Accounting.Infrastructure.Endpoints.PostingBatch.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.PostingBatch;

/// <summary>
/// Endpoint configuration for PostingBatch module.
/// Provides comprehensive REST API endpoints for managing posting-batch.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PostingBatchEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all PostingBatch endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/posting-batch").WithTags("posting-batch");

        group.MapPostingBatchApproveEndpoint();
        group.MapPostingBatchCreateEndpoint();
        group.MapPostingBatchDeleteEndpoint();
        group.MapPostingBatchGetEndpoint();
        group.MapPostingBatchPostEndpoint();
        group.MapPostingBatchRejectEndpoint();
        group.MapPostingBatchReverseEndpoint();
        group.MapPostingBatchSearchEndpoint();
        group.MapPostingBatchUpdateEndpoint();
    }
}
