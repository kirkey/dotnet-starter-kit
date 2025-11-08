namespace Accounting.Application.DeferredRevenues.Update.v1;

public sealed record UpdateDeferredRevenueCommand(
    DefaultIdType Id,
    string? Description = null,
    DateTime? RecognitionDate = null
) : IRequest<DefaultIdType>;
