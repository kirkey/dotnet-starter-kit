namespace Accounting.Application.PostingBatches.Create.v1;

/// <summary>
/// Command to create a new posting batch.
/// </summary>
/// <remarks>
/// PostingBatch Creation:
/// - BatchNumber: Unique identifier for the batch
/// - BatchDate: Effective date for the batch
/// - PeriodId: Optional accounting period reference
/// - Description: Optional batch description
/// 
/// Business Rules:
/// - Created in Draft status with Pending approval
/// - Journal entries can be added after creation
/// - Must be approved before posting
/// </remarks>
public sealed record CreatePostingBatchCommand(
    string BatchNumber,
    DateTime BatchDate,
    DefaultIdType? PeriodId = null,
    string? Description = null,
    string? Notes = null
) : IRequest<PostingBatchCreateResponse>;
