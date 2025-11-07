namespace Accounting.Application.PostingBatches.Get.v1;

/// <summary>
/// Query to retrieve a posting batch by ID.
/// </summary>
public sealed record PostingBatchGetQuery(DefaultIdType Id) : IRequest<PostingBatchGetResponse>;
